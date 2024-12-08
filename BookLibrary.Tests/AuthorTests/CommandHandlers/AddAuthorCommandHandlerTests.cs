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

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Author successfully added."));
            Assert.That(result.Result.Name, Is.EqualTo("Test Author"));
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(4));
        }


        [Test]
        public async Task Handle_ShouldReturnFailure_WhenDuplicateAuthorAdded()
        {

            var handler = new AddAuthorCommandHandler(_fakeRepository);
            var existingAuthor = new Author { Id = Guid.NewGuid(), Name = "Duplicate Author" };
            _fakeDatabase.Authors.Add(existingAuthor);

            var command = new AddAuthorCommand(existingAuthor);


            var result = await handler.Handle(command, default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("An author with this name already exists."));
            Assert.That(result.Result, Is.Null);
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(4));
        }
    }
}
