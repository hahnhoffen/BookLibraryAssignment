using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.AddAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Application.Authors.Queries.GetAuthor;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class AddAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldAddAuthorToDatabase()
        {
            // Arrange
            var handler = new AddAuthorCommandHandler(_fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            var command = new AddAuthorCommand(author);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(1));
            Assert.That(_fakeDatabase.Authors[0].Name, Is.EqualTo("Test Author"));
            Assert.That(result.Name, Is.EqualTo("Test Author"));
        }


        [Test]
        public void Handle_ShouldThrowException_WhenDuplicateAuthorAdded()
        {
            // Arrange
            var handler = new AddAuthorCommandHandler(_fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Duplicate Author" };
            _fakeDatabase.Authors.Add(author);

            var command = new AddAuthorCommand(author);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("An author with this name already exists."));
        }
    }
}
