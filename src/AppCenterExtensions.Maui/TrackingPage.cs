using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AppCenterExtensions.Extensions;

namespace AppCenterExtensions.Maui
{
    /// <summary>
    /// An extension of Page with automatic page tracking to AppCenter Analytics
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TrackingPage : Page
    {
        private Stopwatch stopwatch;

        /// <summary>When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Microsoft.Maui.Controls.Page" /> becoming visible.</summary>
        /// <remarks>To be added.</remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            stopwatch = Stopwatch.StartNew();
            TrackingApplication.TrackAppStart(GetType().Name.ToTrackingEventName());
        }

        /// <summary>When overridden, allows the application developer to customize behavior as the <see cref="T:Microsoft.Maui.Controls.Page" /> disappears.</summary>
        /// <remarks>To be added.</remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (stopwatch != null)
                this.TrackPage(stopwatch.Elapsed);
        }
    }
}