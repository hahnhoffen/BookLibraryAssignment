using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Commands.DeleteUser;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.UserTests.CommandHandlers
{
    [TestFixture]
    public class DeleteUserCommandHandlerTests
    {
        private FakeUserRepository _fakeUserRepository;
        private FakeDatabase _fakeDatabase;
        private User _existingUser;

        [SetUp]
        public void SetUp()
        {
            // Initialize the FakeDatabase
            _fakeDatabase = new FakeDatabase();
            Assert.That(_fakeDatabase, Is.Not.Null, "FakeDatabase failed to initialize.");

            // Clear any existing users
            _fakeDatabase.Users.Clear();

            // Add an existing user to the FakeDatabase
            _existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _fakeDatabase.Users.Add(_existingUser);

            // Initialize the repository with the FakeDatabase
            _fakeUserRepository = new FakeUserRepository(_fakeDatabase);
            Assert.That(_fakeUserRepository, Is.Not.Null, "FakeUserRepository failed to initialize.");
        }


        [Test]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(1), "Initial user count should be 1.");
            var command = new DeleteUserCommand(_existingUser.Id);
            var handler = new DeleteUserCommandHandler(_fakeUserRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True, "OperationResult should indicate success.");
            Assert.That(result.Message, Is.EqualTo("User successfully deleted."));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(0), "User count should be 0 after deletion.");
            Assert.That(!_fakeDatabase.Users.Any(u => u.Id == _existingUser.Id), "Deleted user should not exist.");
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new DeleteUserCommand(Guid.NewGuid());
            var handler = new DeleteUserCommandHandler(_fakeUserRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
        }
    }
}
