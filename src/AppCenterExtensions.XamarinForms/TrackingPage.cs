using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AppCenterExtensions.Extensions;
using Xamarin.Forms;

namespace AppCenterExtensions.XamarinForms
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

            if (stopwatch != null)
                this.TrackPage(stopwatch.Elapsed);
        }
    }
}