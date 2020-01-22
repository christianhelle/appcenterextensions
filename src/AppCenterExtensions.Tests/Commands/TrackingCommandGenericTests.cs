using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using System;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public sealed class TrackingCommandGenericTests : TrackingCommandBaseTests
    {
        protected override void OnSetup(out ITrackingCommand sut)
        {
            var action = new Action<Parameter>(p => executeCallCount++);
            var canExecute = new Func<Parameter, bool>(p => ++canExecuteCallCount > 0);

            sut = new TrackingCommand<Parameter>(
                action,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }
    }
}
