using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Application.Books.Commands.AddBook;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class AddBookCommandHandlerTests
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
        public async Task Handle_ShouldAddBookToDatabase()
        {
            // Arrange
            var handler = new AddBookCommandHandler(_fakeRepository);
            var book = new Book("Test Book");
            var command = new AddBookCommand(book);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Book successfully added."));
            Assert.That(result.Result.Count, Is.EqualTo(4));
            Assert.That(result.Result[3].Title, Is.EqualTo("Test Book"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenDuplicateBookAdded()
        {
            // Arrange
            var handler = new AddBookCommandHandler(_fakeRepository);
            var book = new Book("Duplicate Book");
            _fakeDatabase.Books.Add(book);

            var command = new AddBookCommand(book);

            var result = await handler.Handle(command, default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("A book with this title already exists."));
            Assert.That(result.Result, Is.Null);
        }
    }
}
