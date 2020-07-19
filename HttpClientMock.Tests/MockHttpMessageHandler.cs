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

        private static (HttpStatusCode statusCode, string response) nextResponse;

        public static void RegisterResponse(HttpStatusCode statusCode, string response)
        {
            nextResponse.response = response;
            nextResponse.statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            var res = new HttpResponseMessage
            {
                StatusCode = nextResponse.statusCode,
                Content = new StringContent(nextResponse.response)
            };

            nextResponse.statusCode = default(HttpStatusCode);
            nextResponse.response = null;

            return res;
        }
    }
}