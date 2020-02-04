using System;
using System.Collections.Generic;
using AppCenterExtensions.Extensions;
using AppCenterExtensions.Tests.Infrastructure;
using AppCenterExtensions.XamarinForms;
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
    }
}