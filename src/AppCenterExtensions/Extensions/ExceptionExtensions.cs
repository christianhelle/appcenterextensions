using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Crashes;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionExtensions
    {
        public static void Report(
            this Exception exception,
            IDictionary<string, string> properties = null)
        {
            try
            {
                Crashes.TrackError(exception, properties);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Crashes.TrackError(e);
            }
        }

        public static void Log(this Exception exception)
            => exception.Report();
    }
}
