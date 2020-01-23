using System;
using System.Threading.Tasks;
using Microsoft.AppCenter;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public static class AppCenterSetup
    {
        public static async Task UseAnonymousUserIdAsync()
            => AppCenter.SetUserId(
                await GetSupportKeyAsync());

        public static async Task<string> GetSupportKeyAsync()
            => (await AppCenter.GetInstallIdAsync() ?? Guid.NewGuid())
                .ToString()
                .Substring(0, 8);
    }
}