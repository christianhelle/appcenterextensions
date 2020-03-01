using System.Collections.Generic;

namespace AppCenterExtensions.AppInsights
{
    /// <summary>
    /// This class exposes the HTTP headers key names provided by AppCenterExtensions
    /// </summary>
    public static class TelemetryHeaders
    {
        /// <summary>
        /// Header key for <c>x-supportkey</c>
        /// </summary>
        public const string SupportKeyHeader = "x-supportkey";
        /// <summary>
        /// Header key for <c>x-appcentersdkversion</c>
        /// </summary>
        public const string AppCenterSdkVersionHeader = "x-appcentersdkversion";
        /// <summary>
        /// Header key for <c>x-appcenterinstallid</c>
        /// </summary>
        public const string AppCenterInstallIdHeader = "x-appcenterinstallid";

        /// <summary>
        /// A collection containing the header key name constants exposed by the class.
        /// This is used by <see cref="AppCenterTelemetryInitializer"/> internally
        /// </summary>
        public static readonly IReadOnlyList<string> HeaderKeys
            = new[] {
                SupportKeyHeader,
                AppCenterInstallIdHeader,
                AppCenterSdkVersionHeader
            };
    }
}
