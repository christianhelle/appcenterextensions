using FluentAssertions;
using AppCenterExtensions.AppInsights;
using Xunit;

namespace AppCenterExtensions.Tests.AppInsights
{
    public class TelemetryHeadersTests 
    {
        [Fact]
        public void HeaderKeys_NotNull()
            => TelemetryHeaders.HeaderKeys.Should().NotBeNullOrEmpty();

        [Fact]
        public void HeaderKeys_Contains_SupportKey()
            => TelemetryHeaders.HeaderKeys
                .Should()
                .Contain(TelemetryHeaders.SupportKeyHeader);
        
        [Fact]
        public void HeaderKeys_Contains_AppCenterSdkVersion()
            => TelemetryHeaders.HeaderKeys
                .Should()
                .Contain(TelemetryHeaders.AppCenterSdkVersionHeader);
        
        [Fact]
        public void HeaderKeys_Contain_AppCenterInstallId()
            => TelemetryHeaders.HeaderKeys
                .Should()
                .Contain(TelemetryHeaders.AppCenterInstallIdHeader);
    }
}