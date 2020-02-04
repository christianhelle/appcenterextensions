using System.Collections.Generic;

namespace AppCenterExtensions
{
    public interface IAnalytics
    {
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
