using System;
using System.Collections.Generic;
using AppCenterExtensions.Diagnostics;
using AppCenterExtensions.Tests.Infrastructure;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests.Diagnostics
{
    public class AnalyticsEventTests
    {
        [Theory, AutoMoqData]
        public void EventName_NotNull(AnalyticsEvent sut)
            => sut.EventName.Should().NotBeNullOrEmpty();

        [Theory, AutoData]
        public void EventName_Required(
            IDictionary<string, string> properties)
            => new Action(() => new AnalyticsEvent(null, properties))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void EventName_Set_From_Constructor(
            string eventName,
            IDictionary<string, string> properties)
            => new AnalyticsEvent(eventName, properties)
                .EventName
                .Should()
                .Be(eventName);

        [Theory, AutoMoqData]
        public void Properties_Set_From_Constructor(
            string eventName,
            IDictionary<string, string> properties)
            => new AnalyticsEvent(eventName, properties)
                .Properties
                .Should()
                .BeEquivalentTo(properties);
    }
}