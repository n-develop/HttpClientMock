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
            services.AddHttpClient<BlogClient>();

            var serviceProvider = services.BuildServiceProvider();

            var blogClient = serviceProvider.GetService<BlogClient>();

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