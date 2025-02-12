namespace Shared.BooksController.Response
{
    public class ResponseOfGetBooks
    {
        public static readonly ResponseOfGetBooks Empty = new();

        public IEnumerable<Book> Books { get; set; } = [];

        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string Description { get; set; } = null!;
            public int PageCount { get; set; }
            public string Excerpt { get; set; } = null!;
            public DateTime PublishDate { get; set; }
        }
    }
}
