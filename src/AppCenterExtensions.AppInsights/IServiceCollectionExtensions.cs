using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions.AppInsights
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        public static void AddAppCenterTelemetry(this IServiceCollection services)
            => services.AddSingleton<ITelemetryInitializer, AppCenterTelemetryInitializer>();
    }
}
