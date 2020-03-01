using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions.AppInsights
{
    /// <summary>
    /// Extension methods for enabling Application Insights AppCenter Telemetry
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="AppCenterTelemetryInitializer"/>
        /// as a <see cref="ITelemetryInitializer"/> implementation 
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        public static void AddAppCenterTelemetry(this IServiceCollection services)
            => services.AddSingleton<ITelemetryInitializer, AppCenterTelemetryInitializer>();
    }
}
