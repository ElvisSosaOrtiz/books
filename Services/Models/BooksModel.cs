namespace Services.Models
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public class BooksModel
    {
        public IEnumerable<Book> Books { get; set; } = [];

        public class Book
        {
            [JsonProperty("id")]
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonProperty("title")]
            [JsonPropertyName("title")]
            public string Title { get; set; } = null!;

            [JsonProperty("description")]
            [JsonPropertyName("description")]
            public string Description { get; set; } = null!;

            [JsonProperty("pageCount")]
            [JsonPropertyName("pageCount")]
            public int PageCount { get; set; }

            [JsonProperty("excerpt")]
            [JsonPropertyName("excerpt")]
            public string Excerpt { get; set; } = null!;

            [JsonProperty("publishDate")]
            [JsonPropertyName("publishDate")]
            public DateTime PublishDate { get; set; }
        }
    }
}
