namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServiceContracts;
    using Shared;
    using Shared.BooksController.Request;
    using Shared.BooksController.Response;
    using Shared.Routing;

    [ApiController]
    [Route(BooksControllerRoutes.Root)]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] PaginationFilters paginationFilters)
        {
            var result = await _booksService.GetBooksAsync(paginationFilters);

            if (result is null) return StatusCode(StatusCodes.Status500InternalServerError, result);

            if (result == ResponseOfGetBooks.Empty || !result.Books.Any()) return NoContent();

            return Ok(result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetBookDetails(int id)
        {
            var result = await _booksService.GetBookDetailsAsync(id);

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBook([FromBody] RequestOfManageBook request)
        {
            var result = await _booksService.AddNewBookAsync(request);

            if (result is null) return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> EditBook(int id, [FromBody] RequestOfManageBook request)
        {
            var result = await _booksService.EditBookAsync(id, request);

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> RemoveBook(int id)
        {
            await _booksService.RemoveBookAsync(id);
            return Ok();
        }
    }
}
