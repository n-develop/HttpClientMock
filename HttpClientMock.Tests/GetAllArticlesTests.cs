using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HttpClientMock.Tests
{
    public class GetAllArticlesTests
    {
        [Fact]
        public async Task GivenSuccessResponseFromServer_WhenArticlesRequests_ThenArticlesAreReturned()
        {
            var response = @"[{
  ""title"": ""THE TITLE"",
  ""description"": ""THE DESCRIPTION"",
  ""id"": 1
},
{
  ""title"": ""THE TITLE 2"",
  ""description"": ""THE DESCRIPTION"",
  ""id"": 2
},
{
  ""title"": ""THE TITLE 3"",
  ""description"": ""THE DESCRIPTION"",
  ""id"": 3
}]";
            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.OK);
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var sut = new BlogClient(httpClient);

            var result = await sut.GetAllArticles();

            result.Should().HaveCount(3);
        }
    }
}