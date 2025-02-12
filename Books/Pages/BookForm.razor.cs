namespace Books.Pages
{
    using Books.Routing;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using Shared.BooksController.Request;
    using Shared.BooksController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class BookForm
    {
        [Inject] private HttpClient HttpClient { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter] public int BookId { get; set; }

        [SupplyParameterFromForm] private RequestOfManageBook RequestOfManageBook { get; set; } = new();

        private ResponseOfGetBooks.Book _book = new();

        protected override async Task OnInitializedAsync()
        {
            if (BookId != 0)
            {
                _book = await HttpClient.GetFromJsonAsync<ResponseOfGetBooks.Book>(BooksControllerRoutes.ManageBook(BookId)) ?? new();
                RequestOfManageBook = new()
                {
                    Title = _book.Title,
                    Description = _book.Description,
                    PageCount = _book.PageCount,
                    Excerpt = _book.Excerpt,
                    PublishDate = _book.PublishDate
                };
            }
        }

        private async Task SaveChangesAsync()
        {
            if (BookId != 0)
            {
                var response = await HttpClient.PutAsJsonAsync(BooksControllerRoutes.ManageBook(BookId), RequestOfManageBook);
                await JSRuntime.InvokeVoidAsync("alert", $"Status Code: {response.StatusCode}. Book edited successfully!");
            }
            else
            {
                var response = await HttpClient.PostAsJsonAsync(BooksControllerRoutes.Root, RequestOfManageBook);
                await JSRuntime.InvokeVoidAsync("alert", $"Status Code: {response.StatusCode}. Book created successfully!");
            }

            NavManager.NavigateTo(ClientRoutes.BookList);
        }
    }
}
