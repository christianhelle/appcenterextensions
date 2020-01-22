using ChristianHelle.DeveloperTools.AppCenterExtensions.Command;
using FluentAssertions;
using Microsoft.AppCenter.Analytics;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public class TrackingCommandTests
    {
        private int actionCallCount, canExecuteCallCount;
        private readonly TrackingCommand sut;
        private readonly Action action;
        private readonly Func<bool> canExecute;
        private readonly Mock<IAnalytics> analyticsMock;

        public TrackingCommandTests()
        {
            action = new Action(() => actionCallCount++);
            canExecute = new Func<bool>(() => ++canExecuteCallCount > 0);
            analyticsMock = new Mock<IAnalytics>();

            sut = new TrackingCommand(
                action,
                nameof(TrackingCommandTests),
                "Unit Test",
                canExecute,
                analytics: analyticsMock.Object);

            sut.Execute(new
            {
                Text = "string",
                Number = 1,
                Strings = Array.Empty<string>()
            });
        }

        [Fact]
        public void EventName_NotNull()
            => sut.EventName.Should().NotBeNull();

        [Fact]
        public void ScreenName_NotNull()
            => sut.ScreenName.Should().NotBeNull();

        [Fact]
        public void Properties_NotNull()
            => sut.Properties.Should().NotBeNull();

        [Fact]
        public void Properties_NotBeEmpty()
            => sut.Properties.Should().NotBeEmpty();

        [Theory]
        [InlineData("Text")]
        [InlineData("Number")]
        [InlineData("Strings")]
        public void Properties_Keys_Contain(string key)
            => sut.Properties.Keys.Any(c => c.Contains(key)).Should().BeTrue();

        [Fact]
        public void Callback_Action_Executed()
            => actionCallCount.Should().BeGreaterThan(0);

        [Fact]
        public void Callback_CanExecute_Executed()
            => canExecuteCallCount.Should().BeGreaterThan(0);

        [Fact]
        public void TracksEvent()
            => analyticsMock.Verify(c => c.TrackEvent(nameof(TrackingCommandTests), sut.Properties));
    }
}
