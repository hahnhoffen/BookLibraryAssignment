using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Queries.GetAllBooks;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldReturnAllBooks()
        {
            // Arrange
            var handler = new GetAllBooksQueryHandler(_fakeDatabase);
            _fakeDatabase.Books.Add(new Book("Book 1"));
            _fakeDatabase.Books.Add(new Book("Book 2"));

            var query = new GetAllBooksQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

    }
}
