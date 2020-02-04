using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace AppCenterExtensions
{
    /// <summary>
    /// An interface clone of AppCenter Crashes
    /// </summary>
    public interface ICrashes
    {
        /// <summary>
        /// Track a handled error
        /// </summary>
        /// <param name="exception">Exception thrown</param>
        /// <param name="properties">Custom properties to attach to the error report</param>
        /// <param name="attachments">Custom attachments</param>
        void TrackError(
            Exception exception,
            IDictionary<string, string> properties = null,
            params ErrorAttachmentLog[] attachments);
    }
}