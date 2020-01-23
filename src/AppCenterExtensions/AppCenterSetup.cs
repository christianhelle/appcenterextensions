using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AppCenter;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public interface IAppCenterSetup
    {
        Task UseAnonymousUserIdAsync();
        Task<string> GetSupportKeyAsync();
        
        string AppCenterSdkVersion { get; }
        Task<Guid?> GetAppCenterInstallIdAsync();
    }

    [ExcludeFromCodeCoverage]
    public sealed class AppCenterSetup : IAppCenterSetup
    {
        private static readonly Lazy<AppCenterSetup> lazyInstance = new Lazy<AppCenterSetup>();
        public static AppCenterSetup Instance => lazyInstance.Value;

        public async Task UseAnonymousUserIdAsync()
            => AppCenter.SetUserId(
                await GetSupportKeyAsync());

        public async Task<string> GetSupportKeyAsync()
            => (await AppCenter.GetInstallIdAsync() ?? Guid.NewGuid())
                .ToString()
                .Substring(0, 8);

        public string AppCenterSdkVersion => AppCenter.SdkVersion;
        public Task<Guid?> GetAppCenterInstallIdAsync() => AppCenter.GetInstallIdAsync();
    }
}