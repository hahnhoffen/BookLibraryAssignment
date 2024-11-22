using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Queries.GetAuthor;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetAuthorQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldReturnCorrectAuthor()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeDatabase);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            _fakeDatabase.Authors.Add(author);

            var query = new GetAuthorQuery(author.Id);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Test Author"));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeDatabase);
            var query = new GetAuthorQuery(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(query, default));
            Assert.That(ex.Message, Does.Contain("Author with ID"));
        }
    }
}
