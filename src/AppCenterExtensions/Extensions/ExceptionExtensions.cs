using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Crashes;

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods to the Exception class for logging errors to AppCenter
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Send error to AppCenter
        /// </summary>
        /// <param name="exception">The error</param>
        /// <param name="properties">Custom properties to include in the error report</param>
        /// <param name="crashes">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        public static void Report(
            this Exception exception,
            IDictionary<string, string> properties = null,
            ICrashes crashes = null)
        {
            if (crashes == null)
                crashes = AppCenterCrashes.Instance;
            try
            {
                crashes.TrackError(exception, properties);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                crashes.TrackError(e);
            }
        }
    }
}
