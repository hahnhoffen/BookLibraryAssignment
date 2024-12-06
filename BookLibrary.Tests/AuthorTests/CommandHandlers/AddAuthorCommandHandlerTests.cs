using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.AddAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Application.Authors.Queries.GetAuthor;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class AddAuthorCommandHandlerTests
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
        public async Task Handle_ShouldAddAuthorToDatabase()
        {

            var handler = new AddAuthorCommandHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            var command = new AddAuthorCommand(author);

            var result = await handler.Handle(command, default);

            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(4));

            Assert.That(_fakeDatabase.Authors[3].Name, Is.EqualTo("Test Author"));

            Assert.That(result.Name, Is.EqualTo("Test Author"));
        }


        [Test]
        public void Handle_ShouldThrowException_WhenDuplicateAuthorAdded()
        {
            var handler = new AddAuthorCommandHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Duplicate Author" };
            _fakeDatabase.Authors.Add(author);

            var command = new AddAuthorCommand(author);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("An author with this name already exists."));
        }
    }
}
