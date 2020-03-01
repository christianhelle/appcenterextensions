using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCenterExtensions.AppInsights
{
    public class AppCenterTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;
        private readonly IReadOnlyList<string> headerKeys;

        public AppCenterTelemetryInitializer(IHttpContextAccessor context, string[] headerKeys = null)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.headerKeys = headerKeys ?? TelemetryHeaders.HeaderKeys;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (!(telemetry is ISupportProperties supportProperties))
                return;

            foreach (var headerKey in headerKeys)
            {
                var value = TryGetValue(headerKey);
                if (!string.IsNullOrWhiteSpace(value))
                    supportProperties.Properties[headerKey] = value;
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
