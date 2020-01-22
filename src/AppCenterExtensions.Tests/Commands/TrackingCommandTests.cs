using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using System;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public sealed class TrackingCommandTests : TrackingCommandBaseTests
    {
        protected override void OnSetup(out ITrackingCommand sut)
        {
            var action = new Action(() => executeCallCount++);
            var canExecute = new Func<bool>(() => ++canExecuteCallCount > 0);

            sut = new TrackingCommand(
                action,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }
    }
}
