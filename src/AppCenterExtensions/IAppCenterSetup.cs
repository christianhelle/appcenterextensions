using System;
using System.Threading.Tasks;
using Microsoft.AppCenter;

namespace AppCenterExtensions
{
    /// <summary>
    /// Convenience class for configuring AppCenter Crash Reporting and Analytics
    /// </summary>
    public interface IAppCenterSetup
    {
        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        void Start(string appleSecret, string androidSecret, bool anonymizeUser = false);
        
        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appSecret">AppCenter secrets for all supported platforms</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        void Start(string appSecret, bool anonymizeUser = false);
        
        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <returns>An awaitable task</returns>
        Task StartAsync(string appleSecret, string androidSecret, bool anonymizeUser = false);
        
        /// <summary>
        /// Start AppCenter Crash Reporting and Analytics
        /// </summary>
        /// <param name="appSecret">AppCenter secrets for all supported platforms</param>
        /// <param name="anonymizeUser">Set to TRUE to use a 8 character unique key as the UserId</param>
        /// <returns>An awaitable task</returns>
        Task StartAsync(string appSecret, bool anonymizeUser = false);
        
        /// <summary>
        /// Uses an 8 character unique key as the UserId. This is based on the AppCenter Install ID
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task UseAnonymousUserIdAsync();
        
        /// <summary>
        /// Returns an 8 character unique key. This is designed to be used as an anonymous User ID
        /// </summary>
        /// <returns></returns>
        Task<string> GetSupportKeyAsync();

        /// <summary>
        /// Gets the AppCenter SDK Version
        /// </summary>
        string AppCenterSdkVersion { get; }
        
        /// <summary>
        ///     Get the unique installation identifier for this application installation on this device.
        /// </summary>
        /// <remarks>
        ///     The identifier is lost if clearing application data or uninstalling application.
        /// </remarks>
        Task<Guid?> GetAppCenterInstallIdAsync();
        
        /// <summary>
        /// This property controls the amount of logs emitted by the SDK.
        /// </summary>
        LogLevel LogLevel { get; set; }
    }
}