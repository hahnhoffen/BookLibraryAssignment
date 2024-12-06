using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Queries.GetBook;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetBookQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private FakeBookRepository _fakeRepository;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _fakeRepository = new FakeBookRepository(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnCorrectBook()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeRepository);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var query = new GetBookQuery(book.Id);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Test Book"));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeRepository);
            var query = new GetBookQuery(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(query, default));
            Assert.That(ex.Message, Is.EqualTo("Book not found.")); 
        }
    }
}
