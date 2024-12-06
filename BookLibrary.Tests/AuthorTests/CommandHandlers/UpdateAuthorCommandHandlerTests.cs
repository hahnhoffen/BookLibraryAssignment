using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.UpdateAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class UpdateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private FakeAuthorRepository _fakeRepository;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _fakeRepository = new FakeAuthorRepository(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldUpdateAuthorInDatabase()
        {

            var handler = new UpdateAuthorCommandHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Original Name" };
            _fakeDatabase.Authors.Add(author);

            var updatedAuthor = new Author { Id = author.Id, Name = "Updated Name" };
            var command = new UpdateAuthorCommand(updatedAuthor);

            await handler.Handle(command, default);

            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(4)); // Includes pre-populated authors
            var updated = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == author.Id);
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorToUpdateDoesNotExist()
        {
            // Arrange
            var handler = new UpdateAuthorCommandHandler(_fakeRepository);
            var updatedAuthor = new Author { Id = Guid.NewGuid(), Name = "Non-Existent Author" };
            var command = new UpdateAuthorCommand(updatedAuthor);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("Author not found."));
        }
    }
}
