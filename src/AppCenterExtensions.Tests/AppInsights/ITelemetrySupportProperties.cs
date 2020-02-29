using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace AppCenterExtensions.Tests.AppInsights
{
    public interface ITelemetrySupportProperties : ITelemetry, ISupportProperties
    {
    }
}
