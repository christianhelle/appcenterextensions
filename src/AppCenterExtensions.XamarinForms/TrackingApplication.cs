using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    public class TrackingApplication
    {
        private static bool appStartReported;

        public TrackingApplication(
            string appCenterSecrets,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
        {
            if (appCenterSecrets == null)
                throw new ArgumentNullException(nameof(appCenterSecrets));

            StartAppCenterSdk(
                appCenterSecrets,
                anonymizeAppCenterUser,
                appCenterSetup ?? AppCenterSetup.Instance);
        }

        public TrackingApplication(
            string appleSecret,
            string androidSecret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            : this(
                GetAppCenterSecrets(appleSecret, androidSecret),
                anonymizeAppCenterUser,
                appCenterSetup)
        {
        }

        private static string GetAppCenterSecrets(
            string appleSecret,
            string androidSecret)
            => $"ios={appleSecret};android={androidSecret}";

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

        private static void StartAppCenterSdk(
            string appCenterSecrets,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup)
        {
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