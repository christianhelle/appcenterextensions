using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace AppCenterExtensions.AppInsights
{
    public class AppCenterTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;

        public AppCenterTelemetryInitializer(IHttpContextAccessor context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (!(telemetry is ISupportProperties sp))
            {
                return;
            }

            var supportKey = TryGetValue(TelemetryHeaders.SupportKeyHeader);
            if (!string.IsNullOrWhiteSpace(supportKey))
            {
                sp.Properties[TelemetryHeaders.SupportKeyHeader] = supportKey;
            }

            var appCenterSdkVersion = TryGetValue(TelemetryHeaders.AppCenterSdkVersionHeader);
            if (!string.IsNullOrWhiteSpace(appCenterSdkVersion))
            {
                sp.Properties[TelemetryHeaders.AppCenterSdkVersionHeader] = appCenterSdkVersion;
            }

            var appCenterInstallId = TryGetValue(TelemetryHeaders.AppCenterInstallIdHeader);
            if (!string.IsNullOrWhiteSpace(appCenterSdkVersion))
            {
                sp.Properties[TelemetryHeaders.AppCenterInstallIdHeader] = appCenterSdkVersion;
            }
        }

        private string TryGetValue(string headerKey)
        {
            var httpContext = context?.HttpContext;
            var request = httpContext?.Request;
            var headers = request?.Headers;
            return headers?.TryGetValue(headerKey, out var value) ?? false
                           ? value.FirstOrDefault()
                           : null;
        }
    }
}
