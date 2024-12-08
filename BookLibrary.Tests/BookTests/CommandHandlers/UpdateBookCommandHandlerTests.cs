using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Commands.UpdateBook;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class UpdateBookCommandHandlerTests
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
        public async Task Handle_ShouldUpdateBookInDatabase()
        {
            var handler = new UpdateBookCommandHandler(_fakeRepository);
            var book = new Book("Original Title") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var updatedBook = new Book("Updated Title")
            {
                Id = book.Id,
                AuthorId = Guid.NewGuid(),
                Year = 2024
            };
            var command = new UpdateBookCommand(updatedBook);

            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Book successfully updated."));
            Assert.That(result.Result.Count, Is.EqualTo(4));
            var updated = result.Result.FirstOrDefault(b => b.Id == book.Id);
            Assert.That(updated, Is.Not.Null); 
            Assert.That(updated.Title, Is.EqualTo("Updated Title")); 
            Assert.That(updated.AuthorId, Is.EqualTo(updatedBook.AuthorId)); 
            Assert.That(updated.Year, Is.EqualTo(2024));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookToUpdateDoesNotExist()
        {
            var handler = new UpdateBookCommandHandler(_fakeRepository);
            var updatedBook = new Book("Non-Existent Book") { Id = Guid.NewGuid() };
            var command = new UpdateBookCommand(updatedBook);

            var result = await handler.Handle(command, default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Book not found.")); 
            Assert.That(result.Result, Is.Null); 
        }
    }
}
