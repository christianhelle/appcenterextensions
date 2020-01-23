using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Http
{
    public class DiagnosticDelegatingHandler : DelegatingHandler
    {
        private readonly IAppCenterSetup appCenterSetup;
        private readonly IAnalytics analytics;

        public DiagnosticDelegatingHandler(
            IAnalytics analytics = null,
            IAppCenterSetup appCenterSetup = null)
        {
            this.analytics = analytics ?? new AppCenterAnalytics();
            this.appCenterSetup = appCenterSetup ?? AppCenterSetup.Instance;
        }

        public DiagnosticDelegatingHandler(
            HttpMessageHandler innerHandler,
            IAnalytics analytics = null,
            IAppCenterSetup appCenterSetup = null)
            : base(innerHandler)
        {
            this.analytics = analytics ?? new AppCenterAnalytics();
            this.appCenterSetup = appCenterSetup ?? AppCenterSetup.Instance;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await AddSupportInformationHeaders(request);
            return await SendRequest(request, cancellationToken);
        }

        private async Task AddSupportInformationHeaders(HttpRequestMessage request)
        {
            request.Headers.Add(
                "X-AppCenterSdkVersion",
                appCenterSetup.AppCenterSdkVersion);

            var installIdAsync = await appCenterSetup.GetAppCenterInstallIdAsync();
            if (installIdAsync != null)
            {
                request.Headers.Add(
                    "X-AppCenterInstallId",
                    installIdAsync.ToString());
            }

            request.Headers.Add(
                "X-SupportKey",
                await appCenterSetup.GetSupportKeyAsync());
        }

        private async Task<HttpResponseMessage> SendRequest(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            HttpResponseMessage response = null;
            try
            {
                response = await base.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode) 
                    TrackHttpErrorEvent(request, stopwatch, response);
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                Debug.WriteLine(e);
                TrackHttpErrorEvent(request, stopwatch, response, e);
                throw;
            }
            finally
            {
                if (response != null)
                    TraceResponseTime(request, response, stopwatch);
            }

            return response;
        }

        private static void TraceResponseTime(
            HttpRequestMessage request,
            HttpResponseMessage response,
            Stopwatch stopwatch)
        {
            var status = response.StatusCode;
            var uri = request.RequestUri;
            var duration = stopwatch.ElapsedMilliseconds;
            var uriScheme = uri.Scheme.ToUpper();
            var message = $"{uriScheme} ({duration}ms) - {request.Method} {uri.AbsoluteUri} {(int) status} ({status})";
            Debug.WriteLine(message);
        }

        private void TrackHttpErrorEvent(HttpRequestMessage request,
            Stopwatch stopwatch,
            HttpResponseMessage response = null,
            Exception exception = null)
        {
            var properties = new Dictionary<string, string>
            {
                {"Endpoint", $"{request.Method} {request.RequestUri.AbsoluteUri}"},
                {"Duration", stopwatch.Elapsed.ToString()}
            };

            if (exception != null)
                properties.Add(nameof(Exception), exception.Message);

            if (response != null)
                properties.Add(
                    nameof(HttpStatusCode),
                    $"{response.StatusCode} ({(int) response.StatusCode})");

            analytics.TrackEvent("HTTP Error", properties);
        }
    }
}