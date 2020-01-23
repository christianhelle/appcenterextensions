using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public class DiagnosticDelegatingHandler : DelegatingHandler
    {
        private readonly IAnalytics analytics;

        public DiagnosticDelegatingHandler(IAnalytics analytics = null)
        {
            this.analytics = analytics ?? new AppCenterAnalytics();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await AddSupportInformationHeaders(request);
            return await SendRequest(request, cancellationToken);
        }

        private static async Task AddSupportInformationHeaders(HttpRequestMessage request)
        {
            request.Headers.Add(
                "X-AppCenterSdkVersion",
                AppCenter.SdkVersion);

            var installIdAsync = await AppCenter.GetInstallIdAsync();
            if (installIdAsync != null)
            {
                request.Headers.Add(
                    "X-AppCenterSdkVersion",
                    installIdAsync.ToString());
            }

            request.Headers.Add(
                "X-SupportKey",
                await AppCenterSetup.GetSupportKeyAsync());
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
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                Debug.WriteLine(e);
                TrackHttpErrorEvent(request, e, stopwatch, response);
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

        private void TrackHttpErrorEvent(
            HttpRequestMessage request,
            Exception exception,
            Stopwatch stopwatch,
            HttpResponseMessage response)
        {
            var properties = new Dictionary<string, string>
            {
                {nameof(Exception), exception.Message},
                {"Endpoint", $"{request.Method} {request.RequestUri.AbsoluteUri}"},
                {"Duration", stopwatch.Elapsed.ToString()}
            };

            if (response != null)
            {
                properties.Add(nameof(HttpStatusCode), $"{response.StatusCode} ({(int) response.StatusCode})");
            }

            analytics.TrackEvent("HTTP Error", properties);
        }
    }
}