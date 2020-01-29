using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    public static class TrackingApplication
    {
        private static bool appStartReported;

        private static string GetAppCenterSecrets(
            string appleSecret,
            string androidSecret)
        {
            if (appleSecret == null) throw new ArgumentNullException(nameof(appleSecret));
            if (androidSecret == null) throw new ArgumentNullException(nameof(androidSecret));
            return $"ios={appleSecret};android={androidSecret}";
        }

        public static Stopwatch AppLaunchTime { get; } = Stopwatch.StartNew();

        public static void Initialize(
            string appleSecret,
            string androidSecret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            => StartAppCenterSdk(
                GetAppCenterSecrets(appleSecret, androidSecret),
                anonymizeAppCenterUser,
                appCenterSetup);

        public static void Initialize(
            string secret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            => StartAppCenterSdk(
                secret,
                anonymizeAppCenterUser,
                appCenterSetup);

        private static void StartAppCenterSdk(
            string appCenterSecrets,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup)
        {
            if (appCenterSetup == null)
                appCenterSetup = AppCenterSetup.Instance;

            appCenterSetup.Start(appCenterSecrets);

            if (anonymizeAppCenterUser)
                appCenterSetup.UseAnonymousUserIdAsync();

            appCenterSetup.LogLevel = LogLevel.Verbose;
        }

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