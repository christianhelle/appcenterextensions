using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppCenterExtensions.Tests.Http
{
    public class FailingDelegatingHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => throw new WebException(
                "Test",
                WebExceptionStatus.ConnectFailure);
    }
}