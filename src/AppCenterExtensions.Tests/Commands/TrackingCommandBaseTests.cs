using System;
using System.Linq;
using AppCenterExtensions.Commands;
using FluentAssertions;
using Moq;
using Xunit;

namespace AppCenterExtensions.Tests.Commands
{
    public abstract class TrackingCommandBaseTests
    {
        protected const string EventName = "Unit Test Started";
        protected const string ScreenName = "Unit Test";
        protected int executeCallCount, canExecuteCallCount;
        protected readonly Mock<IAnalytics> analyticsMock;
        protected readonly ITrackingCommand sut;

        protected TrackingCommandBaseTests()
        {
            analyticsMock = new Mock<IAnalytics>();
            OnSetup(out sut);

            sut.Execute(new Parameter
            {
                Text = "string",
                Number = 1,
                Strings = Array.Empty<string>()
            });
        }

        protected abstract void OnSetup(out ITrackingCommand sut);

        protected class Parameter
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
        [InlineData("Text")]
        [InlineData("Number")]
        [InlineData("Strings")]
        public void Properties_Keys_Contain(string key)
            => sut.Properties.Keys.Any(c => c.Contains(key)).Should().BeTrue();

        [Fact]
        public void Callback_Action_Executed()
            => executeCallCount.Should().BeGreaterThan(0);

        [Fact]
        public void Callback_CanExecute_Executed()
            => canExecuteCallCount.Should().BeGreaterThan(0);

        [Fact]
        public void TracksEvent()
            => analyticsMock.Verify(c => c.TrackEvent(EventName, sut.Properties));

        [Fact]
        public void RaiseCanExecuteChanged_Fires_CanExecuteChanged()
        {
            var raised = false;
            sut.CanExecuteChanged += (sender, args) => raised = true;
            sut.RaiseCanExecuteChanged();
            raised.Should().BeTrue();
        }
    }
}
