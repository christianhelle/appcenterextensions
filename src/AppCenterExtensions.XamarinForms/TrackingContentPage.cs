using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AppCenterExtensions.Extensions;
using Xamarin.Forms;

namespace AppCenterExtensions.XamarinForms
{
    /// <summary>
    /// An extension of ContentPage with automatic page tracking to AppCenter Analytics
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TrackingContentPage : ContentPage
    {
        private Stopwatch stopwatch;

        /// <summary>When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.ContentPage" /> becoming visible.</summary>
        /// <remarks>To be added.</remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            stopwatch = Stopwatch.StartNew();
            TrackingApplication.TrackAppStart(GetType().Name.ToTrackingEventName());
        }

        /// <summary>When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.ContentPage" /> disappears.</summary>
        /// <remarks>To be added.</remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (stopwatch != null)
                this.TrackPage(stopwatch.Elapsed);
        }
    }
}