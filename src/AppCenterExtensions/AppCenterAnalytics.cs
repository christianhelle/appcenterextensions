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
            => GetCustomPropertiesAsync(properties)
                .WhenNotNullAsync(t => Analytics.TrackEvent(name, t))
                .Forget();

        private static async Task<IDictionary<string, string>> GetCustomPropertiesAsync(
            IDictionary<string, string> properties)
        {
            if (properties == null)
                properties = new Dictionary<string, string>();
            properties["SupportKey"] = await AppCenterSetup.Instance.GetSupportKeyAsync();
            properties["SessionId"] = AppCenterSetup.Instance.SessionId;
            return properties;
        }
    }
}
