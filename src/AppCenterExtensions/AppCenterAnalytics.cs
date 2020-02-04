using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public static IAnalytics Instance { get; } = new AppCenterAnalytics();

        /// <summary>
        /// Track a custom event
        /// </summary>
        /// <param name="name">Event name</param>
        /// <param name="properties">Custom properties to include in event</param>
        public void TrackEvent(string name, IDictionary<string, string> properties = null) 
            => Analytics.TrackEvent(name, properties);
    }
}
