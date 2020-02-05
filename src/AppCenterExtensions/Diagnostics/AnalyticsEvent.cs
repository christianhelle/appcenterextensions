using System;
using System.Collections.Generic;

namespace AppCenterExtensions.Diagnostics
{
    /// <summary>
    /// This type must be used as the object argument with System.Diagnostics.Trace.Write()
    /// to log to AppCenter Analytics when the AppCenterTraceListener is added to the
    /// current Trace Listeners
    /// </summary>
    public class AnalyticsEvent
    {
        /// <summary>
        /// Creates an instance of AnalyticsEvent
        /// </summary>
        /// <param name="eventName">Event name that describes the event occurring (ex: Logout Button Tapped)</param>
        /// <param name="properties">Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})</param>
        public AnalyticsEvent(
            string eventName,
            IDictionary<string, string> properties = null)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Properties = properties;
        }

        /// <summary>
        /// Gets the event name that describes the event occurring (ex: Logout Button Tapped)
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Gets the custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})
        /// </summary>
        public IDictionary<string, string> Properties { get; }
    }
}