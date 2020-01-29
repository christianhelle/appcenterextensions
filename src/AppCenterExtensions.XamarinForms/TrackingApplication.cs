using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    [ExcludeFromCodeCoverage]
    public class TrackingApplication : Application
    {
        private readonly IAppCenterSetup appCenterSetup;
        public static Stopwatch AppLaunchTime { get; } = Stopwatch.StartNew();

        public TrackingApplication(
            string appCenterSecrets,
            bool anonymizeAppCenterUser,
            IAppCenterSetup appCenterSetup = null)
        {
            this.appCenterSetup = appCenterSetup;
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

        private void StartAppCenterSdk(
            string appCenterSecrets,
            bool anonymizeAppCenterUser)
        {
            try
            {
                appCenterSetup.Start(appCenterSecrets);

                if (anonymizeAppCenterUser)
                    appCenterSetup.UseAnonymousUserIdAsync();

                appCenterSetup.LogLevel = LogLevel.Verbose;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }
    }
}