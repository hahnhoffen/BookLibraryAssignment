using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Queries.GetAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetAuthorQueryHandlerTests
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
        public async Task Handle_ShouldReturnCorrectAuthor()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            _fakeDatabase.Authors.Add(author);

            var query = new GetAuthorQuery(author.Id);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Author"));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeRepository);
            var query = new GetAuthorQuery(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(query, default));
            Assert.That(ex.Message, Does.Contain("Author with ID"));
        }
    }
}
