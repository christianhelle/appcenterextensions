using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCenterExtensions.AppInsights
{
    /// <summary>
    /// Implementation of <see cref="T:Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    /// that adds AppCenter diagnostic information when logging to Application Insights 
    /// </summary>
    public class AppCenterTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;
        private readonly IReadOnlyList<string> headerKeys;

        /// <summary>
        /// Creates an instance <see cref="AppCenterTelemetryInitializer"/>
        /// </summary>
        /// <param name="context">HTTP Context</param>
        /// <param name="headerKeys">HTTP Headers to include when logging to Application Insights</param>
        public AppCenterTelemetryInitializer(IHttpContextAccessor context, string[] headerKeys = null)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.headerKeys = TelemetryHeaders.HeaderKeys.Union(headerKeys ?? Array.Empty<string>()).ToList();
        }

        /// <summary>
        /// Initializes properties of the specified
        /// <see cref="T:Microsoft.ApplicationInsights.Channel.ITelemetry" /> object.
        /// </summary>
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
