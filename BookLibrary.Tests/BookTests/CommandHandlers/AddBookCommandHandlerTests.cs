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
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(4)); // 3 default books + 1 added
            Assert.That(_fakeDatabase.Books[3].Title, Is.EqualTo("Test Book")); // Ensure the last book is the new one
        }

        [Test]
        public void Handle_ShouldThrowException_WhenDuplicateBookAdded()
        {
            // Arrange
            var handler = new AddBookCommandHandler(_fakeRepository);
            var book = new Book("Duplicate Book");
            _fakeDatabase.Books.Add(book);

            var command = new AddBookCommand(book);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("A book with this title already exists."));
        }


    }
}
