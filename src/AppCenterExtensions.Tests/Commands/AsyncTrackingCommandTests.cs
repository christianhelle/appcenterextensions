using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using Moq;
using System;
using System.Threading.Tasks;
using System.Transactions;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public sealed class AsyncTrackingCommandTests : TrackingCommandBaseTests
    {
        private Func<Task> func;
        private Func<bool> canExecute;
        
        protected override void OnSetup(out ITrackingCommand sut)
        {
            func = () =>
            {
                executeCallCount++;
                return Task.CompletedTask;
            };
            canExecute = () => ++canExecuteCallCount > 0;

            sut = new AsyncTrackingCommand(
                func,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }

        [Fact]
        public void CompletionTask_NotNull()
            => ((AsyncTrackingCommand) sut).CompletionTask.Should().NotBeNull();

        [Fact]
        public void CanExecute_False()
        {
            executeCallCount = 0;
            
            new AsyncTrackingCommand(
                func,
                EventName,
                ScreenName,
                () => false,
                analytics: analyticsMock.Object)
                .Execute(null);
            
            executeCallCount.Should().Be(0);
        }

        [Fact]
        public void Requires_ExecuteCallback()
        {
            new Action(
                    () =>
                        new AsyncTrackingCommand(
                            null,
                            EventName,
                            ScreenName,
                            null,
                            analytics: analyticsMock.Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Requires_EventName()
        {
            new Action(
                    () =>
                        new AsyncTrackingCommand(
                            func,
                            null,
                            ScreenName,
                            null,
                            analytics: analyticsMock.Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Takes_CallerType_If_ScreenName_Null()
        {
            new AsyncTrackingCommand(
                    func,
                    EventName,
                    null,
                    null,
                    analytics: analyticsMock.Object)
                .ScreenName
                .Should()
                .Be(GetType().Name.ToTrackingEventName());
        }
    }
}