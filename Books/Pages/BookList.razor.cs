namespace Books.Pages
{
    using Microsoft.AspNetCore.Components;
    using Radzen;
    using Shared.BooksController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class BookList
    {
        [Inject] private HttpClient HttpClient { get; set; } = null!;

        private ResponseOfGetBooks _response = new();

        protected override async Task OnInitializedAsync()
        {
            _response = await GetBooksAsync(1);
        }

        private async Task<ResponseOfGetBooks> GetBooksAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await HttpClient.GetFromJsonAsync<ResponseOfGetBooks>(BooksControllerRoutes.GetBookPaginatedList(pageNumber, pageSize)) ?? ResponseOfGetBooks.Empty;
        }

        private async void PageChangedAsync(PagerEventArgs args)
        {
            _response = await GetBooksAsync(args.PageIndex + 1, args.Top);
            StateHasChanged();
        }
    }
}
