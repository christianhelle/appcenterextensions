using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Xamarin.Forms;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    [ExcludeFromCodeCoverage]
    public class TrackingContentPage : ContentPage
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

            if (stopwatch != null)
                this.TrackPage(stopwatch.Elapsed);
        }
    }
}