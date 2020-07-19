using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HttpClientMock.Tests
{
    public class CreateArticleTests
    {
        [Fact]
        public async Task GivenSuccessResponseFromServer_WhenArticlePosted_ThenSuccessMessageIsReturned()
        {
            var response = @"{
  ""title"": ""THE TITLE"",
  ""description"": ""THE DESCRIPTION"",
  ""id"": 101
}";
            var messageHandler = new MockHttpMessageHandler();
            MockHttpMessageHandler.RegisterResponse(HttpStatusCode.OK, response);
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var sut = new BlogClient(httpClient);

            var result = await sut.CreateArticle(new Article
            {
                Title = "THE TITLE",
                Description = "THE DESCRIPTION"
            });

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Article 'THE TITLE' created with id 101");
            messageHandler.Input.Should().Be(@"{""title"":""THE TITLE"",""description"":""THE DESCRIPTION"",""canonical_url"":null}");
            messageHandler.NumberOfCalls.Should().Be(1);
        }
        
        [Fact]
        public async Task GivenErrorResponseFromServer_WhenArticlePosted_ThenErrorMessageIsReturned()
        {
            var response = string.Empty;
            var messageHandler = new MockHttpMessageHandler();

            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var sut = new BlogClient(httpClient);

            MockHttpMessageHandler.RegisterResponse(HttpStatusCode.InternalServerError, response);

            var result = await sut.CreateArticle(new Article
            {
                Title = "THE TITLE",
                Description = "THE DESCRIPTION"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("That went wrong!");
        }
    }
}