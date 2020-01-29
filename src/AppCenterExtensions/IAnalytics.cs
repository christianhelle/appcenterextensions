using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface IAnalytics
    {
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
