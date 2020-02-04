using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Analytics;

namespace AppCenterExtensions
{
    [ExcludeFromCodeCoverage]
    public class AppCenterAnalytics : IAnalytics
    {
        public static IAnalytics Instance { get; } = new AppCenterAnalytics();

        public void TrackEvent(string name, IDictionary<string, string> properties = null) 
            => Analytics.TrackEvent(name, properties);
    }
}
