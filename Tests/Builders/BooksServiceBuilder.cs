namespace Tests.Builders
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Services;
    using System.Net.Http;

    public class BooksServiceBuilder
    {
        private IHttpClientFactory? _httpClientFactory;
        private ILogger<BooksService>? _logger;

        public BooksService Build()
        {
            var httpClientFactory = _httpClientFactory ?? Mock.Of<IHttpClientFactory>();
            var logger = _logger ?? Mock.Of<ILogger<BooksService>>();

            return new BooksService(httpClientFactory, logger);
        }

        public BooksServiceBuilder With(IHttpClientFactory value)
        {
            _httpClientFactory = value;
            return this;
        }

        public BooksServiceBuilder With(ILogger<BooksService> value)
        {
            _logger = value;
            return this;
        }
    }
}
