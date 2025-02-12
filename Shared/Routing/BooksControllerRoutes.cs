namespace Shared.Routing
{
    public class BooksControllerRoutes
    {
        public const string Root = "api/books";

        public static string ManageBook(int id) => $"{Root}/{id}";
        public static string GetBookPaginatedList(int pageNumber, int pageSize) =>
            $"{Root}?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}";
    }
}
