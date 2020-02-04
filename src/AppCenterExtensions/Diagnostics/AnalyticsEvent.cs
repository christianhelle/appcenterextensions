using System;
using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Diagnostics
{
    public class AnalyticsEvent
    {
        public AnalyticsEvent(
            string eventName,
            IDictionary<string, string> properties = null)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Properties = properties;
        }

        public string EventName { get; }
        public IDictionary<string,string> Properties { get; }
    }
}