using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Queries.GetAllUsers;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.UserTests.QueryHandlers
{
    [TestFixture]
    public class GetAllUsersQueryHandlerTests
    {
        private FakeUserRepository _fakeUserRepository = null!;

        [SetUp]
        public void SetUp()
        {
            var fakeDatabase = new FakeDatabase();
            _fakeUserRepository = new FakeUserRepository(fakeDatabase);

            fakeDatabase.Users.Clear();
            fakeDatabase.Users.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), Username = "User1", Email = "user1@example.com" },
                new User { Id = Guid.NewGuid(), Username = "User2", Email = "user2@example.com" }
            });
        }

        [Test]
        public async Task Handle_ShouldReturnAllUsers()
        {
            var handler = new GetAllUsersQueryHandler(_fakeUserRepository);
            var query = new GetAllUsersQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Result.Count, Is.EqualTo(2));
        }
    }
}

