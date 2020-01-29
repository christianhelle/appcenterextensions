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
        private readonly IAppCenterSetup appCenterSetup;

        public TrackingApplication(
            string appCenterSecrets,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
        {
            if (appCenterSecrets == null)
                throw new ArgumentNullException(nameof(appCenterSecrets));

            this.appCenterSetup = appCenterSetup ?? AppCenterSetup.Instance;
            StartAppCenterSdk(appCenterSecrets, anonymizeAppCenterUser);
        }

        public TrackingApplication(
            string appleSecret,
            string androidSecret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
            : this(
                $"ios={appleSecret};android={androidSecret}",
                anonymizeAppCenterUser,
                appCenterSetup)
        {
        }

        public static Stopwatch AppLaunchTime { get; } = Stopwatch.StartNew();

        public static void Initialize(
            string appleSecret,
            string androidSecret,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
        {
            new TrackingApplication(
                appleSecret,
                androidSecret,
                anonymizeAppCenterUser,
                appCenterSetup);
        }

        private void StartAppCenterSdk(
            string appCenterSecrets,
            bool anonymizeAppCenterUser)
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