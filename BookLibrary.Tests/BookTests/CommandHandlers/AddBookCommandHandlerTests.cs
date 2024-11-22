using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Application.Books.Commands.AddBook;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class AddBookCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldAddBookToDatabase()
        {
            // Arrange
            var handler = new AddBookCommandHandler(_fakeDatabase);
            var book = new Book("Test Book");
            var command = new AddBookCommand(book);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(1));
            Assert.That(_fakeDatabase.Books[0].Title, Is.EqualTo("Test Book"));
        }
        [Test]
        public void Handle_ShouldThrowException_WhenDuplicateBookAdded()
        {
            // Arrange
            var handler = new AddBookCommandHandler(_fakeDatabase);
            var book = new Book("Duplicate Book");
            _fakeDatabase.Books.Add(book);

            var command = new AddBookCommand(book);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("A book with this title already exists."));
        }


    }
}
