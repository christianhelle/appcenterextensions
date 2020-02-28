using AppCenterExtensions.AppInsights;
using AppCenterExtensions.Tests.Infrastructure;
using AutoFixture;
using FluentAssertions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AppCenterExtensions.Tests.AppInsights
{
    public class AppCenterTelemetryInitializerTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly IHeaderDictionary headers = new HeadersDictionary();
        private Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpRequest> mockRequest;

        public AppCenterTelemetryInitializerTests()
        {
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContext = new Mock<HttpContext>();
            mockRequest = new Mock<HttpRequest>();

            headers.Add(TelemetryHeaders.SupportKeyHeader, fixture.Create<StringValues>());
            headers.Add(TelemetryHeaders.AppCenterInstallIdHeader, fixture.Create<StringValues>());
            headers.Add(TelemetryHeaders.AppCenterSdkVersionHeader, fixture.Create<StringValues>());

            mockHttpContextAccessor.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockRequest.Setup(c => c.Headers).Returns(headers);
        }

        [Theory, AutoMoqData]
        public void Initialize_Does_Nothing_If_Args_Not_ISupportProperties(
            AppCenterTelemetryInitializer sut,
            ITelemetry telemetry)
        {
            sut.Initialize(telemetry);
            telemetry.Should().NotBeAssignableTo<ISupportProperties>();
        }

        [Theory, AutoMoqData]
        public void Initialize_Adds_SupportKeyHeader_To_ISupportProperties_Properties(
            ITelemetrySupportProperties properties)
        {
            SetupSut(properties)
                .Should()
                .ContainKey(TelemetryHeaders.SupportKeyHeader);
        }

        [Theory, AutoMoqData]
        public void Initialize_Adds_AppCenterInstallIdHeader_To_ISupportProperties_Properties(
            ITelemetrySupportProperties properties)
        {
            SetupSut(properties)
                .Should()
                .ContainKey(TelemetryHeaders.AppCenterInstallIdHeader);
        }

        [Theory, AutoMoqData]
        public void Initialize_Adds_AppCenterSdkVersionHeader_To_ISupportProperties_Properties(
            ITelemetrySupportProperties properties)
        {
            SetupSut(properties)
                .Should()
                .ContainKey(TelemetryHeaders.AppCenterSdkVersionHeader);
        }

        private Dictionary<string, string> SetupSut(ITelemetrySupportProperties properties)
        {
            var dictionary = new Dictionary<string, string>();
            var mock = Mock.Get(properties);
            mock.Setup(c => c.Properties).Returns(dictionary);

            var sut = new AppCenterTelemetryInitializer(mockHttpContextAccessor.Object);
            sut.Initialize(properties as ITelemetry);
            return dictionary;
        }
    }
}
