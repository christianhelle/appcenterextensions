using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppCenterExtensions.Tests.Http
{
    public class DummyDelegatingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode statusCode;

        public DummyDelegatingHandler(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(
                new HttpResponseMessage(
                    statusCode)
                {
                    RequestMessage = request
                });
    }
}