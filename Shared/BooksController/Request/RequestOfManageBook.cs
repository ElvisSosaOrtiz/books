namespace Shared.BooksController.Request
{
    public class RequestOfManageBook
    {
        public required string Title { get; set; } = null!;
        public required string Description { get; set; } = null!;
        public required int PageCount { get; set; }
        public required string Excerpt { get; set; } = null!;
        public DateTime PublishDate { get; set; }
    }
}
