using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientMock.Tests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler()
        {
        }


        private static AsyncLocal<Queue<(HttpStatusCode statusCode, string response)>> nextResponseStack
            = new AsyncLocal<Queue<(HttpStatusCode statusCode, string response)>>();

        public static void RegisterResponse(HttpStatusCode statusCode, string response)
        {
            nextResponseStack.Value ??= new Queue<(HttpStatusCode statusCode, string response)>();
            nextResponseStack.Value.Enqueue((statusCode, response));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            var r = nextResponseStack.Value.Dequeue();

            return new HttpResponseMessage
            {
                StatusCode = r.statusCode,
                Content = new StringContent(r.response)
            };
        }
    }
}