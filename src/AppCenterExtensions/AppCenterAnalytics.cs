using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public class AppCenterAnalytics : IAnalytics
    {
        public static IAnalytics Instance { get; } = new AppCenterAnalytics();

        public void TrackEvent(string name, IDictionary<string, string> properties = null) 
            => Analytics.TrackEvent(name, properties);
    }
}
