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
        private bool onAppearing, onDisappearing;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (onAppearing)
                return;
            onAppearing = true;
            
            stopwatch = Stopwatch.StartNew();
            TrackAppStart();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (onDisappearing)
                return;
            onDisappearing = true;
            
            Analytics.TrackEvent(
                GetType().Name.ToTrackingEventName(),
                new Dictionary<string, string>
                {
                    {nameof(Title), Title},
                    {"Duration",$"{stopwatch?.Elapsed.TotalSeconds}"}
                });
        }

        private static bool isCalled;
        protected void TrackAppStart()
        {
            if (isCalled)
                return;
            isCalled = true;

            TrackingApplication.AppLaunchTime.Stop();
            Analytics.TrackEvent(
                "App Startup",
                new Dictionary<string, string>
                {
                    {"Duration", TrackingApplication.AppLaunchTime?.Elapsed.ToString()},
                    {"Start Page", GetType().Name.ToTrackingEventName()}
                });
        }
    }
}