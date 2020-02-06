using System;
using System.Threading.Tasks;
using AppCenterExtensions.Extensions;
using AppCenterExtensions.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace AppCenterExtensions.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        [Theory, AutoMoqData]
        public void Forget_Invokes_Crashes_TrackError(
            Exception exception,
            ICrashes crashes)
        {
            var task = Task.Run(() => throw exception);
            task.Forget(true, crashes);
            
            new Func<Task>(() => task)
                .Should()
                .Throw<Exception>();

            Mock.Get(crashes).Verify(c => c.TrackError(exception, null));
        }

        [Theory, AutoMoqData]
        public void Forget_Swallows_Exceptions(
            Exception exception)
            => new Action(
                    () => Task.Run(() => throw exception).Forget())
                .Should()
                .NotThrow();

        [Fact]
        public void Forget_Never_Throws()
            => new Action(
                    () => Task.CompletedTask.Forget())
                .Should()
                .NotThrow();
        
        [Theory, AutoMoqData]
        public void WhenErrorReportAsync_Swallows_Exceptions(
            Exception exception)
            => new Func<Task>(
                    () => Task.Run(() => throw exception).WhenErrorReportAsync())
                .Should()
                .NotThrow();
        
        [Theory, AutoMoqData]
        public async Task WhenErrorReportAsync_Invokes_Crashes_TrackError(
            Exception exception,
            ICrashes crashes)
        {
            await Task.Run(() => throw exception).WhenErrorReportAsync(crashes);
            Mock.Get(crashes).Verify(c => c.TrackError(exception, null));
        }
        
        [Theory, AutoMoqData]
        public async Task WhenErrorReportAsync_Generic_Invokes_Crashes_TrackError(
            Exception exception,
            ICrashes crashes)
        {
            await Task.Run(() => FailingGenericTask(exception))
                .WhenErrorReportAsync(crashes);
            Mock.Get(crashes).Verify(c => c.TrackError(exception, null));
        }

        private static Task<string> FailingGenericTask(Exception exception)
            => throw exception; 
    }
}