using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface IAppCenterSetup
    {
        void Start(string appleSecret, string androidSecret, bool anonymizeUser = false);
        void Start(string appSecret, bool anonymizeUser = false);
        Task StartAsync(string appleSecret, string androidSecret, bool anonymizeUser = false);
        Task StartAsync(string appSecret, bool anonymizeUser = false);
        Task UseAnonymousUserIdAsync();
        Task<string> GetSupportKeyAsync();

        string AppCenterSdkVersion { get; }
        Task<Guid?> GetAppCenterInstallIdAsync();
        LogLevel LogLevel { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class AppCenterSetup : IAppCenterSetup
    {
        private static readonly Lazy<AppCenterSetup> LazyInstance = new Lazy<AppCenterSetup>();
        public static AppCenterSetup Instance => LazyInstance.Value;

        public void Start(string appleSecret, string androidSecret, bool anonymizeUser = false)
            => Start(GetSecrets(appleSecret, androidSecret), anonymizeUser);

        public void Start(string appSecret, bool anonymizeUser = false)
            => StartAsync(appSecret, anonymizeUser).Forget();

        public Task StartAsync(string appleSecret, string androidSecret, bool anonymizeUser = false)
            => StartAsync(GetSecrets(appleSecret, androidSecret), anonymizeUser);

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

        public async Task UseAnonymousUserIdAsync()
            => AppCenter.SetUserId(
                await GetSupportKeyAsync());

        public async Task<string> GetSupportKeyAsync()
            => (await AppCenter.GetInstallIdAsync() ?? Guid.NewGuid())
                .ToString()
                .Substring(0, 8);

        public string AppCenterSdkVersion => AppCenter.SdkVersion;

        public Task<Guid?> GetAppCenterInstallIdAsync() => AppCenter.GetInstallIdAsync();

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