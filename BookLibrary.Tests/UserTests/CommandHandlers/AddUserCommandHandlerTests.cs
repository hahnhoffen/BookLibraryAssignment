using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Commands.AddUser;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using BookLibrary.Application.Users.Queries.GetUser;

namespace BookLibrary.Tests.UserTests.CommandHandlers
{
    [TestFixture]
    public class AddUserCommandHandlerTests
    {
        private FakeUserRepository _fakeUserRepository;
        private FakeDatabase _fakeDatabase;
        private ILogger<AddUserCommandHandler> _logger;

        [SetUp]
        public void SetUp()
        {

            _fakeDatabase = new FakeDatabase();
            _fakeDatabase.Users.Clear();

            _fakeDatabase.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });

            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            }).CreateLogger<AddUserCommandHandler>();
            _fakeUserRepository = new FakeUserRepository(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var command = new AddUserCommand(new User
            {
                Id = Guid.NewGuid(),
                Username = "newuser",
                Email = "newuser@example.com",
                PasswordHash = "newhashedpassword",
                CreatedAt = DateTime.UtcNow
            });
            var handler = new AddUserCommandHandler(_fakeUserRepository, new PasswordService(), _logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User added successfully."));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(2));
            Assert.That(_fakeDatabase.Users.Any(u => u.Username == "newuser"), Is.True);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUsernameOrEmailIsTaken()
        {
            // Arrange
            var command = new AddUserCommand(new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "newemail@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });
            var handler = new AddUserCommandHandler(_fakeUserRepository, new PasswordService(), _logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username or email is already taken."));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(1));
        }
    }
}
