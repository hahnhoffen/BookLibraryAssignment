using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Queries.GetBook;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetBookQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldReturnCorrectBook()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeDatabase);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var query = new GetBookQuery(book.Id);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo("Test Book"));
        }
        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeDatabase);
            var query = new GetBookQuery(Guid.NewGuid()); // Non-existent ID

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
