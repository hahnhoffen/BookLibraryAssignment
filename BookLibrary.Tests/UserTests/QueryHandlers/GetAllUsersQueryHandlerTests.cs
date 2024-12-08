using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Queries.GetAllUsers;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Tests.UserTests.QueryHandlers
{
    [TestFixture]
    public class GetAllUsersQueryHandlerTests
    {
        private FakeUserRepository _fakeUserRepository = null!;
        private ILogger<GetAllUsersQueryHandler> _logger = null!;

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
            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            }).CreateLogger<GetAllUsersQueryHandler>();
        }

        [Test]
        public async Task Handle_ShouldReturnAllUsers()
        {
            var handler = new GetAllUsersQueryHandler(_fakeUserRepository, _logger);
            var query = new GetAllUsersQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Result.Count, Is.EqualTo(2));
        }
    }
}

