using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientMock.Tests
{
    public class DoubleRegisterTests
    {
        [Fact]
        public async Task Test_Double_Register_Returned_In_Right_Order()
        {
            HttpClient client = new HttpClient(new MockHttpMessageHandler());

            MockHttpMessageHandler.RegisterResponse(HttpStatusCode.Accepted, "abc");
            MockHttpMessageHandler.RegisterResponse(HttpStatusCode.BadRequest, "def");

            var first = await client.GetAsync("https://www.google.com");
            var firstStr = await first.Content.ReadAsStringAsync();

            var second = await client.GetAsync("https://www.google.com");
            var secondStr = await second.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Accepted, first.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);

            Assert.Equal("abc", firstStr);
            Assert.Equal("def", secondStr);




        }
    }
}