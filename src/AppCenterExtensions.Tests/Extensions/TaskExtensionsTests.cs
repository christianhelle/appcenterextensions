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
        public void Invoke_Crashes_TrackError(
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
        public void Forget_Never_Throws(
            Exception exception,
            ICrashes crashes)
            => new Action(
                    () => Task.Run(() => throw exception).Forget(true, crashes))
                .Should()
                .NotThrow();
    }
}