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
            var handler = new DeleteBookCommandHandler(_fakeRepository);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var command = new DeleteBookCommand(book.Id);

            var result = await handler.Handle(command, default);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Book successfully deleted."));
            Assert.That(result.Result.Count, Is.EqualTo(3));
            Assert.That(!result.Result.Any(b => b.Id == book.Id));
        }


        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookToDeleteDoesNotExist()
        {
            // Arrange
            var handler = new DeleteBookCommandHandler(_fakeRepository);
            var command = new DeleteBookCommand(Guid.NewGuid());

            // Act & Assert
            var result = await handler.Handle(command, default);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Book not found."));
            Assert.That(result.Result, Is.Null);
        }
    }
}
