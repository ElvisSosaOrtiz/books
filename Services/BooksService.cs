namespace Services
{
    using Microsoft.Extensions.Logging;
    using ServiceContracts;
    using Services.Constants;
    using Services.ExternalApiRouting;
    using Services.Models;
    using Shared;
    using Shared.BooksController.Request;
    using Shared.BooksController.Response;
    using System.Collections.Generic;
    using System.Net.Http.Json;

    public class BooksService : IBooksService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BooksService> _logger;

        public BooksService(
            IHttpClientFactory httpClientFactory,
            ILogger<BooksService> logger)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.ExternalApiClient);
            _logger = logger;
        }

        public async Task<ResponseOfGetBooks?> GetBooksAsync(PaginationFilters paginationFilters)
        {
            try
            {
                var response = await _httpClient.GetAsync(BooksApiRouting.Root);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(response.Content.ToString());
                    return null;
                }

                var books = await response.Content.ReadFromJsonAsync<IEnumerable<BookModel>>();

                return new()
                {
                    Count = books!.Count(),
                    Books = books!.Select(book => new ResponseOfGetBooks.Book
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        PageCount = book.PageCount,
                        Excerpt = book.Excerpt,
                        PublishDate = book.PublishDate
                    })
                    .Skip((paginationFilters.PageNumber - 1) * paginationFilters.PageSize)
                    .Take(paginationFilters.PageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetBooks.Book?> GetBookDetailsAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(BooksApiRouting.ManageBooks(id));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(response.Content.ToString());
                    return null;
                }

                var book = (await response.Content.ReadFromJsonAsync<BookModel>())!;

                return new()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    PageCount = book.PageCount,
                    Excerpt = book.Excerpt,
                    PublishDate = book.PublishDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetBooks.Book?> AddNewBookAsync(RequestOfManageBook request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BooksApiRouting.Root, request);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(response.Content.ToString());
                    return null;
                }

                var book = (await response.Content.ReadFromJsonAsync<BookModel>())!;

                return new()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    PageCount = book.PageCount,
                    Excerpt = book.Excerpt,
                    PublishDate = book.PublishDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetBooks.Book?> EditBookAsync(int id, RequestOfManageBook request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(BooksApiRouting.ManageBooks(id), request);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(response.Content.ToString());
                    return null;
                }

                var book = (await response.Content.ReadFromJsonAsync<BookModel>())!;

                return new()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    PageCount = book.PageCount,
                    Excerpt = book.Excerpt,
                    PublishDate = book.PublishDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task RemoveBookAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(BooksApiRouting.ManageBooks(id));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
