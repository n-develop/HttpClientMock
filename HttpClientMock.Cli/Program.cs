using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientMock.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<BlogClient>();
            services.AddHttpClient<BlogClient>(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));
            var serviceProvider = services.BuildServiceProvider();

            var blogClient = serviceProvider.GetService<BlogClient>();

            await CreateArticle(blogClient);

            await ShowAllArticles(blogClient);
        }

        private static async Task ShowAllArticles(BlogClient blogClient)
        {
            var articles = await blogClient.GetAllArticles();
            foreach (var art in articles)
            {
                Console.WriteLine(art.Title);
            }
        }

        private static async Task CreateArticle(BlogClient blogClient)
        {
            var article = new Article
            {
                Title = "My awesome blog post",
                Description = "This is a super awesome post about stuff I know."
            };

            var result = await blogClient.CreateArticle(article);
            Console.WriteLine(result.Message);
        }
    }
}