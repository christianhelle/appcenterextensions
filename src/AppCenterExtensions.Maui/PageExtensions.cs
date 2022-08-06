using System.Globalization;
using AppCenterExtensions.Extensions;

namespace AppCenterExtensions.Maui
{
    /// <summary>
    /// Exposes extension methods for the Page class for page tracking using AppCenter Analytics 
    /// </summary>
    public static class PageExtensions
    {
        private const string DurationKey = "Duration";
        private static readonly Dictionary<string, string> AdditionalTrackingInfo = new Dictionary<string, string>();

        /// <summary>
        /// Track the Page in AppCenter Analytics
        /// </summary>
        /// <param name="page">Page to track</param>
        /// <param name="duration">
        /// Duration spent on the screen.
        /// This can be calculated by starting a Stopwatch upon OnAppearing and stopping it upon OnDisappearing
        /// </param>
        /// <param name="analytics">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        public static void TrackPage(this Page page, TimeSpan duration, IAnalytics analytics = null)
        {
            var properties = new Dictionary<string, string>
            {
                [nameof(Page.Title)] = page.Title,
                [DurationKey] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture)
            };

            foreach (var additionalTrackingInfo in AdditionalTrackingInfo)
            {
                properties[additionalTrackingInfo.Key] = additionalTrackingInfo.Value;
            }

            (analytics ?? AppCenterAnalytics.Instance)
                .TrackEvent(
                    page.GetType().Name.ToTrackingEventName(),
                    properties);
        }

        /// <summary>
        /// Track any additonal information.
        /// </summary>
        /// <param name="key">Key of the additional information</param>
        /// <param name="value">Value of the additional information</param>
        public static void AddAdditionalTrackingInfo(string key, string value)
        {
            if (key == DurationKey)
                return;
            AdditionalTrackingInfo[key] = value;
        }

        /// <summary>
        /// Get the currently configured additional tracking info
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> GetAdditionalTrackingInfo()
            => AdditionalTrackingInfo;
    }
}