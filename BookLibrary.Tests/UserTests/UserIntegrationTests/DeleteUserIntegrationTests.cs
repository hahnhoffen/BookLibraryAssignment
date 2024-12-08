using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Users.Commands.DeleteUser;
using BookLibrary.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Tests.UserIntegrationTests
{
    [TestFixture]
    public class DeleteUserIntegrationTests : IDisposable
    {
        private RealDataBase _realDatabase;
        private RealUserRepository _realUserRepository;
        private DeleteUserCommandHandler _handler;
        private ILogger<DeleteUserCommandHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RealDataBase>()
                .UseSqlServer("Server=DESKTOP-FSSLAO9\\SQLEXPRESS;Database=BookLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .Options;

            _realDatabase = new RealDataBase(options);

            // Clear the database for a clean test setup
            _realDatabase.Database.EnsureDeleted();
            _realDatabase.Database.EnsureCreated();

            // Add a user for testing
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _realDatabase.Users.Add(testUser);
            _realDatabase.SaveChanges();

            _realUserRepository = new RealUserRepository(_realDatabase);

            // Add the logger instance
            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<DeleteUserCommandHandler>();

            _handler = new DeleteUserCommandHandler(_realUserRepository, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            _realDatabase?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var user = await _realDatabase.Users.FirstOrDefaultAsync(u => u.Username == "existinguser");
            Assert.That(user, Is.Not.Null, "Test setup failed to add user to the database.");

            var command = new DeleteUserCommand(user!.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User successfully deleted."));
            var userInDatabase = await _realDatabase.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.That(userInDatabase, Is.Null);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();
            var command = new DeleteUserCommand(nonExistentUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
        }

        public void Dispose()
        {
            _realDatabase?.Dispose();
        }
    }
}
