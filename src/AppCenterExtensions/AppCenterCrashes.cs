using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Crashes;

namespace AppCenterExtensions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class AppCenterCrashes : ICrashes
    {
        /// <summary>
        /// Singleton default implementation
        /// </summary>
        public static ICrashes Instance { get; } = new AppCenterCrashes();

        /// <summary>
        /// Track a handled error
        /// </summary>
        /// <param name="exception">Exception thrown</param>
        /// <param name="properties">Custom properties to attach to the error report</param>
        /// <param name="attachments">Custom attachments</param>
        public void TrackError(
            Exception exception,
            IDictionary<string, string> properties = null,
            params ErrorAttachmentLog[] attachments)
            => Crashes.TrackError(exception, properties, attachments);
    }
}