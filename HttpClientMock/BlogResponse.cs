using System.Text.Json.Serialization;

namespace HttpClientMock
{
    public class BlogResponse : Article
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}