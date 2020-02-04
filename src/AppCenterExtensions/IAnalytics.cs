using System.Collections.Generic;

namespace AppCenterExtensions
{
    /// <summary>
    /// An interface clone of AppCenter Analytics
    /// </summary>
    public interface IAnalytics
    {
        /// <summary>
        /// Track a custom event
        /// </summary>
        /// <param name="name">Event name</param>
        /// <param name="properties">Custom properties to include in event</param>
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
