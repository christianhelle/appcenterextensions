using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using Moq;
using System;
using System.Threading.Tasks;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public sealed class AsyncTrackingCommandTests : TrackingCommandBaseTests
    {
        protected override void OnSetup(out ITrackingCommand sut)
        {
            var func = new Func<Task>(() => { executeCallCount++; return Task.CompletedTask; });
            var canExecute = new Func<bool>(() => ++canExecuteCallCount > 0);

            sut = new AsyncTrackingCommand(
                func,
                EventName,
                ScreenName,
                canExecute,
                analytics: analyticsMock.Object);
        }
    }
}
