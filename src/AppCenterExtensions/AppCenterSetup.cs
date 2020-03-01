using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AppCenterExtensions.Extensions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AppCenterExtensions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public sealed class AppCenterSetup : IAppCenterSetup
    {
        private volatile string supportKey;

        /// <summary>
        /// Singleton default instance
        /// </summary>
        public static AppCenterSetup Instance
            => Singleton<AppCenterSetup>.GetInstance();

        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        public void Start(string appleSecret, string androidSecret, bool anonymizeUser = false)
            => Start(GetSecrets(appleSecret, androidSecret), anonymizeUser);

        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appSecret">AppCenter secrets for all supported platforms</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        public void Start(string appSecret, bool anonymizeUser = false)
            => StartAsync(appSecret, anonymizeUser).Forget();

        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <returns>An awaitable task</returns>
        public Task StartAsync(string appleSecret, string androidSecret, bool anonymizeUser = false)
            => StartAsync(GetSecrets(appleSecret, androidSecret), anonymizeUser);

        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appSecret">AppCenter secrets for all supported platforms</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <returns>An awaitable task</returns>
        public async Task StartAsync(string appSecret, bool anonymizeUser = false)
        {
            AppCenter.Start(
                appSecret,
                typeof(Analytics),
                typeof(Crashes));

            if (Debugger.IsAttached)
                LogLevel = LogLevel.Verbose;

            if (anonymizeUser)
                await UseAnonymousUserIdAsync();
        }

        /// <summary>
        /// Uses an 8 character unique key as the UserId. This is based on the AppCenter Install ID
        /// </summary>
        /// <returns>An awaitable task</returns>
        public async Task UseAnonymousUserIdAsync()
            => AppCenter.SetUserId(
                await GetSupportKeyAsync());

        /// <summary>
        /// Returns an 8 character unique key. This is designed to be used as an anonymous User ID
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSupportKeyAsync()
        {
            if (!string.IsNullOrWhiteSpace(supportKey))
                return supportKey;

            return supportKey =
                (await AppCenter.GetInstallIdAsync() ?? Guid.NewGuid())
                .ToString()
                .Substring(0, 8);
        }

        /// <summary>
        /// Gets the AppCenter SDK Version
        /// </summary>
        public string AppCenterSdkVersion => AppCenter.SdkVersion;

        /// <summary>
        ///     Get the unique installation identifier for this application installation on this device.
        /// </summary>
        /// <remarks>
        ///     The identifier is lost if clearing application data or uninstalling application.
        /// </remarks>
        public Task<Guid?> GetAppCenterInstallIdAsync() => AppCenter.GetInstallIdAsync();

        /// <summary>
        /// This property controls the amount of logs emitted by the SDK.
        /// </summary>
        public LogLevel LogLevel
        {
            get => AppCenter.LogLevel;
            set => AppCenter.LogLevel = value;
        }

        private static string GetSecrets(string appleSecret, string androidSecret)
            => new AppCenterSecretsBuilder()
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .Build();
    }
}