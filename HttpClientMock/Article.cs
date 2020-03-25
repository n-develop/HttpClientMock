using System.Text.Json.Serialization;

namespace HttpClientMock
{
    public class Article
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("canonical_url")]
        public string CanonicalUrl { get; set; }
    }
}