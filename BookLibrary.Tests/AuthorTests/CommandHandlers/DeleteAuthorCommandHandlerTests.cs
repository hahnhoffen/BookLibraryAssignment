using BookLibrary.Domain.Entities;
using BookLibrary.Application.Authors.Commands.DeleteAuthor;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Application.Common;

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
            var handler = new DeleteAuthorCommandHandler(_fakeRepository);
            var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
            _fakeDatabase.Authors.Add(author);

            var command = new DeleteAuthorCommand(author.Id);

            var result = await handler.Handle(command, default);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Author successfully deleted."));
            Assert.That(result.Result.Count, Is.EqualTo(3));
            Assert.That(!result.Result.Any(a => a.Id == author.Id));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorToDeleteDoesNotExist()
        {

            var handler = new DeleteAuthorCommandHandler(_fakeRepository);
            var command = new DeleteAuthorCommand(Guid.NewGuid());

            var result = await handler.Handle(command, default);


            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Author not found."));
            Assert.That(result.Result, Is.Null);
        }
    }
}
