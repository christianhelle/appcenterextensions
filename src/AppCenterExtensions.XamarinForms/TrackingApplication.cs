using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    public static class TrackingApplication
    {
        private static bool appStartReported;

        public static Stopwatch AppLaunchTime { get; } = Stopwatch.StartNew();

        public static void Initialize(
            string appleSecret,
            string androidSecret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            => (appCenterSetup ?? AppCenterSetup.Instance)
                .Start(appleSecret, androidSecret, anonymizeAppCenterUser);

        public static void Initialize(
            string secret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            => (appCenterSetup ?? AppCenterSetup.Instance)
                .Start(secret, anonymizeAppCenterUser);

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