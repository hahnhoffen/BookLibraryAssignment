using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Queries.GetAllBooks;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
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
        public async Task Handle_ShouldReturnAllBooks()
        {
            // Arrange
            var handler = new GetAllBooksQueryHandler(_fakeRepository);
            _fakeDatabase.Books.Add(new Book("Book 1") { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), Year = 2023 });
            _fakeDatabase.Books.Add(new Book("Book 2") { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), Year = 2023 });

            var query = new GetAllBooksQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Books retrieved successfully."));
            Assert.That(result.Result.Count, Is.EqualTo(5));
            Assert.That(result.Result.Any(b => b.Title == "Book 1"));
            Assert.That(result.Result.Any(b => b.Title == "Book 2"));
        }

    }
}
