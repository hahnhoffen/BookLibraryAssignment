using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.DeleteAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class DeleteAuthorCommandHandlerTests
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
        public async Task Handle_ShouldDeleteAuthorFromDatabase()
        {
            // Arrange
            var handler = new DeleteAuthorCommandHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            _fakeDatabase.Authors.Add(author);

            var command = new DeleteAuthorCommand(author.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(3));
            Assert.That(!_fakeDatabase.Authors.Any(a => a.Id == author.Id));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorToDeleteDoesNotExist()
        {
            // Arrange
            var handler = new DeleteAuthorCommandHandler(_fakeRepository);
            var command = new DeleteAuthorCommand(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("Author not found."));
        }
    }
}
