using System;
using System.Collections.Generic;
using System.Text;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface IAnalytics
    {
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
