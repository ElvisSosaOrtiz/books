namespace Tests
{
    using AutoFixture;
    using Moq;
    using Moq.Protected;
    using Services.Constants;
    using Services.Models;
    using Shared.BooksController.Request;
    using System.Net;
    using System.Net.Http.Json;
    using Tests.Builders;

    [TestFixture]
    public class BooksServiceTests
    {
        public class GetBooksAsyncTests
        {
            [Test]
            public async Task WhenStatusCodeIsNotSuccessful_ShouldReturnNull()
            {
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(It.IsAny<JsonContent>(), HttpStatusCode.InternalServerError))
                    .Build();

                var actual = await instance.GetBooksAsync(new() { PageNumber = 1, PageSize = 10 });

                Assert.IsNull(actual);
            }

            [Test]
            public async Task ShouldReturnBookList()
            {
                var fixture = new Fixture();
                var books = fixture.Create<IEnumerable<BookModel>>();
                var jsonContent = JsonContent.Create(books);
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(jsonContent, HttpStatusCode.OK))
                    .Build();

                var actual = await instance.GetBooksAsync(new() { PageNumber = 1, PageSize = 10 });

                Assert.IsTrue(actual!.Books.All(act => books.Any(exp => exp.Id == act.Id)));
            }
        }

        public class GetBookDetailsAsync
        {
            [Test]
            public async Task WhenStatusCodeIsNotSuccessful_ShouldReturnNull()
            {
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(It.IsAny<JsonContent>(), HttpStatusCode.InternalServerError))
                    .Build();

                var actual = await instance.GetBookDetailsAsync(It.IsAny<int>());

                Assert.IsNull(actual);
            }

            [Test]
            public async Task ShouldReturnBookDetails()
            {
                var fixture = new Fixture();
                var book = fixture.Create<BookModel>();
                var jsonContent = JsonContent.Create(book);
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(jsonContent, HttpStatusCode.OK))
                    .Build();

                var actual = (await instance.GetBookDetailsAsync(book.Id))!;

                Assert.That(actual.Id, Is.EqualTo(book.Id));
            }
        }

        public class AddNewBookAsync
        {
            [Test]
            public async Task WhenStatusCodeIsNotSuccessful_ShouldReturnNull()
            {
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(It.IsAny<JsonContent>(), HttpStatusCode.InternalServerError))
                    .Build();

                var actual = await instance.AddNewBookAsync(It.IsAny<RequestOfManageBook>());

                Assert.IsNull(actual);
            }

            [Test]
            public async Task ShouldReturnCreatedBook()
            {
                var fixture = new Fixture();
                var book = fixture.Create<BookModel>();
                var jsonContent = JsonContent.Create(book);
                var request = fixture.Create<RequestOfManageBook>();
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(jsonContent, HttpStatusCode.OK))
                    .Build();

                var actual = (await instance.AddNewBookAsync(request))!;

                Assert.That(actual.Id, Is.EqualTo(book.Id));
            }
        }

        public class EditBookAsync
        {
            [Test]
            public async Task WhenStatusCodeIsNotSuccessful_ShouldReturnNull()
            {
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(It.IsAny<JsonContent>(), HttpStatusCode.InternalServerError))
                    .Build();

                var actual = await instance.EditBookAsync(It.IsAny<int>(), It.IsAny<RequestOfManageBook>());

                Assert.IsNull(actual);
            }

            [Test]
            public async Task ShouldReturnEditedBook()
            {
                const int BookId = 1;
                var fixture = new Fixture();
                var request = fixture.Create<RequestOfManageBook>();
                var book = new BookModel
                {
                    Id = BookId,
                    Title = request.Title,
                    Description = request.Description,
                    PageCount = request.PageCount,
                    Excerpt = request.Excerpt,
                    PublishDate = request.PublishDate
                };
                var jsonContent = JsonContent.Create(book);
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(jsonContent, HttpStatusCode.OK))
                    .Build();

                var actual = (await instance.EditBookAsync(BookId, request))!;

                Assert.That(actual.Title, Is.EqualTo(request.Title));
            }
        }

        public class RemoveBookAsync
        {
            [Test]
            public void ShouldRemoveBook()
            {
                var instance = new BooksServiceBuilder()
                    .With(MockHttpClientFactory(It.IsAny<JsonContent>(), HttpStatusCode.OK))
                    .Build();

                Assert.DoesNotThrowAsync(() => instance.RemoveBookAsync(It.IsAny<int>()));
            }
        }

        private static IHttpClientFactory MockHttpClientFactory(JsonContent jsonContent, HttpStatusCode statusCode)
        {
            const string ExternalApiBaseURL = "https://external-api.test/";
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = jsonContent,
                StatusCode = statusCode
            };
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();
            var externalApiHttpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(ExternalApiBaseURL) };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(mock => mock.CreateClient(HttpClientNames.ExternalApiClient))
                .Returns(externalApiHttpClient);

            return httpClientFactoryMock.Object;
        }
    }
}