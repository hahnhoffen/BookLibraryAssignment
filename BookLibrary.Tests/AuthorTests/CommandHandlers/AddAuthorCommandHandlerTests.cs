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
            var fakeDatabase = new FakeDatabase();
            var handler = new AddAuthorCommandHandler(fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            var command = new AddAuthorCommand(author);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.That(fakeDatabase.Authors.Count, Is.EqualTo(1)); // Verify that the author was added
            Assert.That(fakeDatabase.Authors[0].Name, Is.EqualTo("Test Author")); // Verify the correct author
            Assert.That(result.Name, Is.EqualTo("Test Author")); // Verify the returned result
        }


        [Test]
        public async Task Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeDatabase);
            var nonExistentAuthorId = Guid.NewGuid();
            var query = new GetAuthorQuery(nonExistentAuthorId);

            try
            {
                // Act
                await handler.Handle(query, default);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                // Assert
                Assert.That(ex.Message, Does.Contain($"Author with ID {nonExistentAuthorId} not found."));
            }
        }

    }
}
