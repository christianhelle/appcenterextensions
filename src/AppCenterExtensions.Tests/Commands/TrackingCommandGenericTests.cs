using ChristianHelle.DeveloperTools.AppCenterExtensions.Command;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Commands
{
    public class TrackingCommandGenericTests
    {
        private int actionCallCount, canExecuteCallCount;
        private readonly TrackingCommand<Parameter> sut;
        private readonly Action<Parameter> action;
        private readonly Func<Parameter, bool> canExecute;
        private readonly Mock<IAnalytics> analyticsMock;

        public TrackingCommandGenericTests()
        {
            action = new Action<Parameter>(p => actionCallCount++);
            canExecute = new Func<Parameter, bool>(p => ++canExecuteCallCount > 0);
            analyticsMock = new Mock<IAnalytics>();

            sut = new TrackingCommand<Parameter>(
                action,
                nameof(TrackingCommandTests),
                "Unit Test",
                canExecute,
                analytics: analyticsMock.Object);

            sut.Execute(new Parameter
            {
                Text = "string",
                Number = 1,
                Strings = Array.Empty<string>()
            });
        }

        private class Parameter
        {
            public string Text { get; internal set; }
            public int Number { get; internal set; }
            public string[] Strings { get; internal set; }
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
        [InlineData(nameof(Parameter.Text))]
        [InlineData(nameof(Parameter.Number))]
        [InlineData(nameof(Parameter.Strings))]
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
