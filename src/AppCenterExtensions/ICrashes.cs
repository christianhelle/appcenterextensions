using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface ICrashes
    {
        void TrackError(
            Exception exception,
            IDictionary<string, string> properties = null,
            params ErrorAttachmentLog[] attachments);
    }
}