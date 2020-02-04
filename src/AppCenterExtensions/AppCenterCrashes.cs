using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Crashes;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    [ExcludeFromCodeCoverage]
    public class AppCenterCrashes : ICrashes
    {
        public static ICrashes Instance { get; } = new AppCenterCrashes();
        
        public void TrackError(
            Exception exception,
            IDictionary<string, string> properties = null,
            params ErrorAttachmentLog[] attachments)
            => Crashes.TrackError(exception, properties, attachments);
    }
}