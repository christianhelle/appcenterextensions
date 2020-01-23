using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    [ExcludeFromCodeCoverage]
    public class AppCenterAnalytics : IAnalytics
    {
        public static IAnalytics Instance { get; } = new AppCenterAnalytics();

        public void TrackEvent(string name, IDictionary<string, string> properties = null) 
            => Analytics.TrackEvent(name, properties);
    }
}
