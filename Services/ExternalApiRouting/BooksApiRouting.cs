namespace Services.ExternalApiRouting
{
    public class BooksApiRouting
    {
        public const string Root = "/api/v1/Books";
        public static string ManageBooks(int id) => $"{Root}/{id}";
    }
}
