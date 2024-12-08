using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Application.Users.Commands.UpdateUser;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.UserTests.CommandHandlers
{
    [TestFixture]
    public class UpdateUserCommandHandlerTests
    {
        private FakeUserRepository _fakeUserRepository;
        private FakeDatabase _fakeDatabase;
        private User _existingUser;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _fakeDatabase.Users.Clear();

            _existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _fakeDatabase.Users.Add(_existingUser);

            _fakeUserRepository = new FakeUserRepository(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldUpdateUser_WhenUserExists()
        {
            var updatedUser = new User
            {
                Id = _existingUser.Id,
                Username = "updateduser",
                Email = "updateduser@example.com",
                PasswordHash = "updatedhashedpassword",
                CreatedAt = _existingUser.CreatedAt
            };

            var command = new UpdateUserCommand(updatedUser);
            var handler = new UpdateUserCommandHandler(_fakeUserRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User updated successfully."));
            var userInDatabase = _fakeDatabase.Users.First(u => u.Id == _existingUser.Id);
            Assert.That(userInDatabase.Username, Is.EqualTo("updateduser"));
            Assert.That(userInDatabase.Email, Is.EqualTo("updateduser@example.com"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            var nonExistentUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "nonexistentuser",
                Email = "nonexistentuser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            var command = new UpdateUserCommand(nonExistentUser);
            var handler = new UpdateUserCommandHandler(_fakeUserRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
        }
    }
}
