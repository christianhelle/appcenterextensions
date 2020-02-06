using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Analytics;

namespace AppCenterExtensions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class AppCenterAnalytics : IAnalytics
    {
        /// <summary>
        /// Singleton default implementation
        /// </summary>
        public static IAnalytics Instance => Singleton<AppCenterAnalytics>.GetInstance();

        /// <summary>
        /// Track a custom event
        /// </summary>
        /// <param name="name">Event name</param>
        /// <param name="properties">Custom properties to include in event</param>
        public void TrackEvent(string name, IDictionary<string, string> properties = null)
            => SetSupportKeyAsync(properties)
                .WhenNotNullAsync(t => Analytics.TrackEvent(name, t))
                .Forget();

        private static async Task<IDictionary<string, string>> SetSupportKeyAsync(
            IDictionary<string, string> properties)
        {
            var supportKey = await AppCenterSetup.Instance.GetSupportKeyAsync();
            if (properties == null)
                properties = new Dictionary<string, string>();
            properties["SupportKey"] = supportKey;
            return properties;
        }
    }
}
