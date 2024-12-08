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
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Test Author"));
            Assert.That(result.Result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var handler = new GetAuthorQueryHandler(_fakeRepository);
            var query = new GetAuthorQuery(Guid.NewGuid());


            var result = await handler.Handle(query, default);
            
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("Author with ID"));
            Assert.That(result.Result, Is.Null);
        }
    }
}
