using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using System;
using FluentAssertions;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public sealed class TrackingCommandGenericTests : TrackingCommandBaseTests
    {
        private Action<Parameter> action;
        private Func<Parameter, bool> canExecute;
        
        protected override void OnSetup(out ITrackingCommand sut)
        {
            action = p => executeCallCount++;
            canExecute = p => ++canExecuteCallCount > 0;

            sut = new TrackingCommand<Parameter>(
                action,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }

        [Fact]
        public void CanExecute_False()
        {
            bool Execute(Parameter p)
            {
                executeCallCount = 0;
                ++canExecuteCallCount;
                return false;
            }

            new TrackingCommand<Parameter>(
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
                        new TrackingCommand<Parameter>(
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
                        new TrackingCommand<Parameter>(
                            action,
                            null,
                            ScreenName,
                            null,
                            analytics: analyticsMock.Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Requires_ScreenName()
        {
            new Action(
                    () =>
                        new TrackingCommand<Parameter>(
                            action,
                            EventName,
                            null,
                            null,
                            analytics: analyticsMock.Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
    }
}
