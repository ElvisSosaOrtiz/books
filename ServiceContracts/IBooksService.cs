namespace ServiceContracts
{
    using Shared;
    using Shared.BooksController.Request;
    using Shared.BooksController.Response;
    using System.Threading.Tasks;

    public interface IBooksService
    {
        Task<ResponseOfGetBooks.Book?> AddNewBookAsync(RequestOfManageBook request);
        Task<ResponseOfGetBooks.Book?> EditBookAsync(int id, RequestOfManageBook request);
        Task<ResponseOfGetBooks.Book?> GetBookDetailsAsync(int id);
        Task<ResponseOfGetBooks?> GetBooksAsync(PaginationFilters paginationFilters);
        Task RemoveBookAsync(int id);
    }
}
