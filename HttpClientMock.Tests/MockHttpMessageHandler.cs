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


        private static AsyncLocal<Stack<(HttpStatusCode statusCode, string response)>> nextResponseStack
            = new AsyncLocal<Stack<(HttpStatusCode statusCode, string response)>>();

        public static void RegisterResponse(HttpStatusCode statusCode, string response)
        {
            nextResponseStack.Value ??= new Stack<(HttpStatusCode statusCode, string response)>();
            nextResponseStack.Value.Push((statusCode, response));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            var r = nextResponseStack.Value.Pop();

            var res = new HttpResponseMessage
            {
                StatusCode = r.statusCode,
                Content = new StringContent(r.response)
            };

            return res;
        }
    }
}