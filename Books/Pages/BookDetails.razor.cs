namespace Books.Pages
{
    using Books.Routing;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using Shared.BooksController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class BookDetails
    {
        [Inject] private HttpClient HttpClient { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter] public int BookId { get; set; }

        private ResponseOfGetBooks.Book _book = new();

        protected override async Task OnInitializedAsync()
        {
            _book = await HttpClient.GetFromJsonAsync<ResponseOfGetBooks.Book>(BooksControllerRoutes.ManageBook(BookId)) ?? new();
        }

        private async Task RemoveBookAsync(int id)
        {
            var response = await HttpClient.DeleteAsync(BooksControllerRoutes.ManageBook(id));
            if (response.IsSuccessStatusCode)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Status Code: {response.StatusCode}. Book deleted successfully!");
            }
            NavManager.NavigateTo(ClientRoutes.BookList);
        }
    }
}
