namespace Shared.BooksController.Request
{
    using System.ComponentModel.DataAnnotations;

    public class RequestOfManageBook
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int PageCount { get; set; }

        [Required]
        public string Excerpt { get; set; } = null!;

        [Required]
        public DateTime PublishDate { get; set; }
    }
}
