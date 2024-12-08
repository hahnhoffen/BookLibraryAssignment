using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Users.Commands.UpdateUser;
using MediatR;
using BookLibrary.Application.Common;
using System.Threading.Tasks;
using BookLibrary.Infrastructure.Services;

namespace BookLibrary.Tests.UserIntegrationTests
{
    [TestFixture]
    public class UpdateUserIntegrationTests : IDisposable
    {
        private RealDataBase _realDatabase;
        private RealUserRepository _realUserRepository;
        private UpdateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RealDataBase>()
                .UseSqlServer("Server=DESKTOP-FSSLAO9\\SQLEXPRESS;Database=BookLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .Options;

            _realDatabase = new RealDataBase(options);
            _realUserRepository = new RealUserRepository(_realDatabase);
            _handler = new UpdateUserCommandHandler(_realUserRepository, new PasswordService());

            // Ensure database is clean before each test
            _realDatabase.Database.EnsureDeleted();
            _realDatabase.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _realDatabase?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldUpdateUserInDatabase()
        {
            // Arrange
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "integrationuser",
                Email = "integrationuser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _realDatabase.Users.Add(existingUser);
            await _realDatabase.SaveChangesAsync();

            var updatedUser = new User
            {
                Id = existingUser.Id,
                Username = "updateduser",
                Email = "updateduser@example.com",
                PasswordHash = "newpassword"
            };

            var command = new UpdateUserCommand(updatedUser);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User updated successfully."));

            var userInDatabase = await _realDatabase.Users.FirstOrDefaultAsync(u => u.Id == existingUser.Id);
            Assert.That(userInDatabase, Is.Not.Null);
            Assert.That(userInDatabase!.Username, Is.EqualTo("updateduser"));
            Assert.That(userInDatabase.Email, Is.EqualTo("updateduser@example.com"));
            Assert.That(userInDatabase.PasswordHash, Is.Not.EqualTo("newpassword")); // Ensure password was hashed
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "nonexistent",
                Email = "nonexistent@example.com",
                PasswordHash = "password"
            };

            var command = new UpdateUserCommand(nonExistentUser);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser1 = new User
            {
                Id = Guid.NewGuid(),
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            var existingUser2 = new User
            {
                Id = Guid.NewGuid(),
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _realDatabase.Users.AddRange(existingUser1, existingUser2);
            _realDatabase.SaveChanges();

            var userToUpdate = new User
            {
                Id = existingUser1.Id,
                Username = "updateduser",
                Email = "user2@example.com", // Conflicting email
                PasswordHash = "newhashedpassword",
                CreatedAt = existingUser1.CreatedAt
            };

            var command = new UpdateUserCommand(userToUpdate);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email is already taken."));
        }

        public void Dispose()
        {
            _realDatabase?.Dispose();
        }
    }
}
