using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Commands.AddUser;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Services;

namespace BookLibrary.Tests.UserTests.CommandHandlers
{
    [TestFixture]
    public class AddUserCommandHandlerTests
    {
        private FakeUserRepository _fakeUserRepository;
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void SetUp()
        {
            // Initialize the FakeDatabase
            _fakeDatabase = new FakeDatabase();
            _fakeDatabase.Users.Clear();

            // Add an existing user for duplicate testing
            _fakeDatabase.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });

            // Initialize the repository
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
            var handler = new AddUserCommandHandler(_fakeUserRepository, new PasswordService());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User added successfully."));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(2)); // 1 existing user + 1 new user
            Assert.That(_fakeDatabase.Users.Any(u => u.Username == "newuser"), Is.True);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUsernameOrEmailIsTaken()
        {
            // Arrange
            var command = new AddUserCommand(new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser", // Duplicate username
                Email = "newemail@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });
            var handler = new AddUserCommandHandler(_fakeUserRepository, new PasswordService());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username or email is already taken."));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(1)); // No new user added
        }
    }
}
