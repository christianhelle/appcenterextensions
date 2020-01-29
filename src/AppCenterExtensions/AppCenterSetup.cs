using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface IAppCenterSetup
    {
        void Start(string appSecret);
        Task UseAnonymousUserIdAsync();
        Task<string> GetSupportKeyAsync();
        
        string AppCenterSdkVersion { get; }
        Task<Guid?> GetAppCenterInstallIdAsync();
        LogLevel LogLevel { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class AppCenterSetup : IAppCenterSetup
    {
        private static readonly Lazy<AppCenterSetup> lazyInstance = new Lazy<AppCenterSetup>();
        public static AppCenterSetup Instance => lazyInstance.Value;

        public void Start(string appSecret) 
            => AppCenter.Start(
                appSecret, 
                typeof(Analytics),
                typeof(Crashes));

        public async Task UseAnonymousUserIdAsync()
            => AppCenter.SetUserId(
                await GetSupportKeyAsync());

        public async Task<string> GetSupportKeyAsync()
            => (await AppCenter.GetInstallIdAsync() ?? Guid.NewGuid())
                .ToString()
                .Substring(0, 8);

        public string AppCenterSdkVersion => AppCenter.SdkVersion;
        public Task<Guid?> GetAppCenterInstallIdAsync() => AppCenter.GetInstallIdAsync();
        public LogLevel LogLevel { get; set; } = LogLevel.Verbose;
    }
}