using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.UpdateAuthor;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class UpdateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldUpdateAuthorInDatabase()
        {
            // Arrange
            var handler = new UpdateAuthorCommandHandler(_fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Original Name" };
            _fakeDatabase.Authors.Add(author);

            var updatedAuthor = new Author { Id = author.Id, Name = "Updated Name" };
            var command = new UpdateAuthorCommand(updatedAuthor);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorToUpdateDoesNotExist()
        {
            // Arrange
            var handler = new UpdateAuthorCommandHandler(_fakeDatabase);
            var updatedAuthor = new Author { Id = Guid.NewGuid(), Name = "Non-Existent Author" };
            var command = new UpdateAuthorCommand(updatedAuthor);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
