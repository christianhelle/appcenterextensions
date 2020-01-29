using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    [ExcludeFromCodeCoverage]
    public class TrackingPage : Page
    {
        private Stopwatch stopwatch;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            stopwatch = Stopwatch.StartNew();
            TrackingApplication.TrackAppStart(GetType().Name.ToTrackingEventName());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Analytics.TrackEvent(
                GetType().Name.ToTrackingEventName(),
                new Dictionary<string, string>
                {
                    {nameof(Title), Title},
                    {"Duration", $"{stopwatch?.Elapsed.TotalSeconds}"}
                });
        }
    }
}