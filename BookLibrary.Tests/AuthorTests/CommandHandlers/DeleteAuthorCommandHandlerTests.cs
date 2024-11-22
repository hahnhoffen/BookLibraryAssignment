using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.DeleteAuthor;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class DeleteAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldDeleteAuthorFromDatabase()
        {
            // Arrange
            var handler = new DeleteAuthorCommandHandler(_fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            _fakeDatabase.Authors.Add(author);

            var command = new DeleteAuthorCommand(author.Id);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorToDeleteDoesNotExist()
        {
            // Arrange
            var handler = new DeleteAuthorCommandHandler(_fakeDatabase);
            var command = new DeleteAuthorCommand(Guid.NewGuid());

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
