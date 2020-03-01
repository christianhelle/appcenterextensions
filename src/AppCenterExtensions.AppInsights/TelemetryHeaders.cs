using System.Collections.Generic;

namespace AppCenterExtensions.AppInsights
{
    public static class TelemetryHeaders
    {
        public const string SupportKeyHeader = "x-supportkey";
        public const string AppCenterSdkVersionHeader = "x-appcentersdkversion";
        public const string AppCenterInstallIdHeader = "x-appcenterinstallid";

        public readonly static IReadOnlyList<string> HeaderKeys
            = new[] {
                SupportKeyHeader,
                AppCenterInstallIdHeader,
                AppCenterSdkVersionHeader
            };
    }
}
