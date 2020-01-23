using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Http;
using Moq;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Http
{
    public class DiagnosticDelegatingHandlerTests
    {
        private readonly Mock<IAnalytics> mockAnalytics = new Mock<IAnalytics>();
        private readonly Mock<IAppCenterSetup> mockAppCenterSetup = new Mock<IAppCenterSetup>();
        private readonly Uri requestUri = new Fixture().Create<Uri>();

        private Task<HttpResponseMessage> Prepare(HttpMessageHandler handler)
            => new HttpClient(
                    new DiagnosticDelegatingHandler(
                        handler,
                        mockAnalytics.Object,
                        mockAppCenterSetup.Object))
                .GetAsync(requestUri);

        [Fact]
        public void TrackEvent_Invoked_For_Failed_Requests_Due_To_Exception()
            => Prepare(new FailingDelegatingHandler())
                .ContinueWith(t => VerifyAnalyticsTrackEvent());

        [Fact]
        public void TrackEvent_Invoked_For_Failed_Requests()
            => Prepare(new DummyDelegatingHandler(HttpStatusCode.BadRequest))
                .ContinueWith(t => VerifyAnalyticsTrackEvent());

        [Fact]
        public void TrackEvent_Never_Invoked_For_Success()
            => Prepare(new DummyDelegatingHandler(HttpStatusCode.OK))
                .ContinueWith(t => VerifyAnalyticsTrackEvent(Times.Never()));

        [Fact]
        public void AppCenterSdkVersion_Invoked()
            => Prepare(new DummyDelegatingHandler(HttpStatusCode.OK))
                .ContinueWith(t => mockAppCenterSetup.Verify(
                    c => c.AppCenterSdkVersion));

        [Fact]
        public void GetSupportKey_Invoked()
            => Prepare(new DummyDelegatingHandler(HttpStatusCode.OK))
                .ContinueWith(t => mockAppCenterSetup.Verify(
                    c => c.GetSupportKeyAsync()));

        [Fact]
        public void GetAppCenterInstallIdAsync_Invoked()
            => Prepare(new DummyDelegatingHandler(HttpStatusCode.OK))
                .ContinueWith(t => mockAppCenterSetup.Verify(
                    c => c.GetAppCenterInstallIdAsync()));

        private void VerifyAnalyticsTrackEvent(Times? times = null)
        {
            mockAnalytics.Verify(
                c => c.TrackEvent(
                    "HTTP Error",
                    It.IsAny<Dictionary<string, string>>()),
                times ?? Times.Once());
        }
    }
}