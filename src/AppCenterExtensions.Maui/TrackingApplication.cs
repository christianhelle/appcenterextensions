using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Analytics;

namespace AppCenterExtensions.Maui
{
    /// <summary>
    /// Helper class for tracking application start time 
    /// </summary>
    public static class TrackingApplication
    {
        private static bool appStartReported;

        /// <summary>
        /// A stopwatch that represents the applications uptime used for reporting the startup time
        /// </summary>
        public static Stopwatch AppLaunchTime { get; } = Stopwatch.StartNew();

        /// <summary>
        /// Convenience method for initializing AppCenter
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <param name="appCenterSetup">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        public static void Initialize(
            string appleSecret,
            string androidSecret,
            bool anonymizeUser,
            IAppCenterSetup appCenterSetup = null)
            => (appCenterSetup ?? AppCenterSetup.Instance)
                .Start(appleSecret, androidSecret, anonymizeUser);

        /// <summary>
        /// Convenience method for initializing AppCenter
        /// </summary>
        /// <param name="secret">AppCenter secrets</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <param name="appCenterSetup">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        public static void Initialize(
            string secret,
            bool anonymizeUser,
            IAppCenterSetup appCenterSetup = null)
            => (appCenterSetup ?? AppCenterSetup.Instance)
                .Start(secret, anonymizeUser);

        /// <summary>
        /// Convenience method for reporting application start time
        /// </summary>
        /// <param name="startPage">Name of the applications main landing page</param>
        [ExcludeFromCodeCoverage]
        public static void TrackAppStart(string startPage)
        {
            if (appStartReported)
                return;
            appStartReported = true;

            AppLaunchTime.Stop();

            Analytics.TrackEvent(
                "App Startup",
                new Dictionary<string, string>
                {
                    {"Duration", AppLaunchTime.Elapsed.ToString()},
                    {"Start Page", startPage}
                });
        }
    }
}