using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Users.Queries.GetUser;
using MediatR;
using System.Threading.Tasks;

namespace BookLibrary.Tests.UserIntegrationTests
{
    [TestFixture]
    public class GetUserIntegrationTests : IDisposable
    {
        private RealDataBase _realDatabase;
        private RealUserRepository _realUserRepository;
        private GetUserQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RealDataBase>()
                .UseSqlServer("Server=DESKTOP-FSSLAO9\\SQLEXPRESS;Database=BookLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .Options;

            _realDatabase = new RealDataBase(options);

            _realUserRepository = new RealUserRepository(_realDatabase);
            _handler = new GetUserQueryHandler(_realUserRepository);

            // Ensure database is clean and seeded before each test
            _realDatabase.Database.EnsureDeleted();
            _realDatabase.Database.EnsureCreated();

            // Seed a user
            _realDatabase.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });
            _realDatabase.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _realDatabase?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var existingUser = await _realDatabase.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            Assert.That(existingUser, Is.Not.Null, "Seeded user not found in database.");
            var query = new GetUserQuery(existingUser.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User retrieved successfully."));
            Assert.That(result.Result.Id, Is.EqualTo(existingUser.Id));
            Assert.That(result.Result.Username, Is.EqualTo("testuser"));
            Assert.That(result.Result.Email, Is.EqualTo("testuser@example.com"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();
            var query = new GetUserQuery(nonExistentUserId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
            Assert.That(result.Result, Is.Null);
        }

        public void Dispose()
        {
            _realDatabase?.Dispose();
        }
    }
}
