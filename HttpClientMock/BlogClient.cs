using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientMock
{
    public class BlogClient
    {
        private readonly HttpClient _httpClient;

        public BlogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreationResult> CreateArticle(Article article)
        {
            var serializedArticle = JsonSerializer.Serialize(article);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://jsonplaceholder.typicode.com/posts"),
                Method = HttpMethod.Post,
                Content = new StringContent(serializedArticle, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new CreationResult
                {
                    Success = false,
                    Message = "That went wrong!"
                };
            }

            var stringResponse = await response.Content.ReadAsStringAsync();
            var blogResponse = JsonSerializer.Deserialize<BlogResponse>(stringResponse);

            return new CreationResult
            {
                Success = true,
                Message = $"Article '{article.Title}' created with id {blogResponse.Id}"
            };
        }
    }
}