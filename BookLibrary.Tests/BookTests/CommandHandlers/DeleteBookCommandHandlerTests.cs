using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Commands.DeleteBook;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
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
        public async Task Handle_ShouldDeleteBookFromDatabase()
        {
            // Arrange
            var handler = new DeleteBookCommandHandler(_fakeRepository);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var command = new DeleteBookCommand(book.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(3));
            Assert.That(!_fakeDatabase.Books.Any(b => b.Id == book.Id));
        }


        [Test]
        public void Handle_ShouldThrowException_WhenBookToDeleteDoesNotExist()
        {
            // Arrange
            var handler = new DeleteBookCommandHandler(_fakeRepository);
            var command = new DeleteBookCommand(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("Book not found."));
        }
    }
}
