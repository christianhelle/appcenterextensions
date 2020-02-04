using System;
using AppCenterExtensions.Commands;
using AppCenterExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests.Commands
{
    public sealed class TrackingCommandTests : TrackingCommandBaseTests
    {
        private Action action;
        private Func<bool> canExecute;

        protected override void OnSetup(out ITrackingCommand sut)
        {
            action = () => executeCallCount++;
            canExecute = () => ++canExecuteCallCount > 0;

            sut = new TrackingCommand(
                action,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }

        [Fact]
        public void CanExecute_False()
        {
            bool Execute()
            {
                executeCallCount = 0;
                ++canExecuteCallCount;
                return false;
            }

            new TrackingCommand(
                    action,
                    EventName,
                    ScreenName,
                    Execute,
                    analytics: analyticsMock.Object)
                .Execute(new Parameter());

            executeCallCount.Should().Be(0);
        }

        [Fact]
        public void Requires_ExecuteCallback()
        {
            new Action(
                    () =>
                        new TrackingCommand(
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
                        new TrackingCommand(
                            action,
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
            new TrackingCommand(
                    action,
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