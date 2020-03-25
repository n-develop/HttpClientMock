using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            //https://jsonplaceholder.typicode.com/posts
            var response = await _httpClient.GetAsync("posts");
            var responseBody = await response.Content.ReadAsStringAsync();

            var articles = JsonSerializer.Deserialize<IEnumerable<Article>>(responseBody);

            return articles;
        }

        public async Task<CreationResult> CreateArticle(Article article)
        {
            var serializedArticle = JsonSerializer.Serialize(article);

            var httpContent = new StringContent(serializedArticle, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("posts", httpContent);
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