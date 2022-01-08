using System;
using System.Collections.Generic;
using AppCenterExtensions.Extensions;
using AppCenterExtensions.Tests.Infrastructure;
using AppCenterExtensions.XamarinForms;
using FluentAssertions;
using Moq;
using Xamarin.Forms;
using Xunit;

namespace AppCenterExtensions.Tests.XamarinForms
{
    public class PageExtensionsTests
    {
        public PageExtensionsTests() => MockXamarinForms.Init();

        [Theory, AutoMoqData]
        public void TrackPage_Invokes_Analytics_TrackEvent(
            TimeSpan duration,
            IAnalytics analytics)
        {
            var page = new Page();
            page.TrackPage(duration, analytics);

            Mock.Get(analytics)
                .Verify(
                    c => c.TrackEvent(
                        page.GetType().Name.ToTrackingEventName(),
                        It.IsAny<Dictionary<string, string>>()));
        }

        [Theory, AutoMoqData]
        public void TrackPage_Includes_Duration(
            TimeSpan duration,
            IAnalytics analytics)
        {
            var page = new Page();
            page.TrackPage(duration, analytics);

            Mock.Get(analytics)
                .Verify(
                    c => c.TrackEvent(
                        page.GetType().Name.ToTrackingEventName(),
                        It.Is<Dictionary<string, string>>(
                            dictionary => dictionary.ContainsKey("Duration"))));
        }

        [Theory, AutoMoqData]
        public void TrackPage_Includes_AdditionalTrackingInfo(
            TimeSpan duration,
            IAnalytics analytics,
            Dictionary<string, string> additionalTrackingInfo)
        {
            foreach (var (key, value) in additionalTrackingInfo)
            {
                PageExtensions.AddAdditionalTrackingInfo(key, value);
            }

            var page = new Page();
            page.TrackPage(duration, analytics);

            Mock.Get(analytics)
                .Verify(
                    c => c.TrackEvent(
                        page.GetType().Name.ToTrackingEventName(),
                        It.Is<Dictionary<string, string>>(
                            dictionary => ContainsAdditionTrackingInfo(
                                dictionary,
                                additionalTrackingInfo))));
        }

        private static bool ContainsAdditionTrackingInfo(
            IReadOnlyDictionary<string, string> dictionary,
            Dictionary<string, string> additionalTrackingInfo)
        {
            foreach (var (key, value) in additionalTrackingInfo)
            {
                if (!dictionary.ContainsKey(key))
                    return false;
            }

            return true;
        }

        [Fact]
        public void Duration_AdditionalTrackingInfo_Cannot_Be_Set()
        {
            PageExtensions.AddAdditionalTrackingInfo("Duration", Guid.Empty.ToString());
            PageExtensions.GetAdditionalTrackingInfo().ContainsKey("Duration").Should().BeFalse();
        }
    }
}