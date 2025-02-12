namespace Books.Routing
{
    public class ClientRoutes
    {
        public const string BookList = "/";
        public const string BookForm = "/book-form";

        public static string BookDetails(int id) => $"/book-details/{id}";
        public static string EditBook(int id) => $"{BookForm}/{id}";
    }
}
