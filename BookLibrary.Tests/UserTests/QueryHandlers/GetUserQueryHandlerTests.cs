using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Queries.GetUser;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Tests.UserTests.QueryHandlers
{
    [TestFixture]
    public class GetUserQueryHandlerTests
    {
        private FakeUserRepository _fakeUserRepository;
        private User _existingUser;
        private ILogger<GetUserQueryHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            var fakeDatabase = new FakeDatabase();
            _fakeUserRepository = new FakeUserRepository(fakeDatabase);

            _existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "ExistingUser",
                Email = "existinguser@example.com"
            };

            fakeDatabase.Users.Add(_existingUser);
            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<GetUserQueryHandler>();
        }

        [Test]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            var handler = new GetUserQueryHandler(_fakeUserRepository, _logger);
            var query = new GetUserQuery(_existingUser.Id);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.Username, Is.EqualTo("ExistingUser"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            var handler = new GetUserQueryHandler(_fakeUserRepository, _logger);
            var query = new GetUserQuery(Guid.NewGuid());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
        }
    }
}