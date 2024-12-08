using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Users.Queries.GetAllUsers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BookLibrary.Tests.UserIntegrationTests
{
    [TestFixture]
    public class GetAllUsersIntegrationTests : IDisposable
    {
        private RealDataBase _realDatabase;
        private RealUserRepository _realUserRepository;
        private GetAllUsersQueryHandler _handler;
        private ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<RealDataBase>()
                .UseSqlServer("Server=DESKTOP-FSSLAO9\\SQLEXPRESS;Database=BookLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .Options;

            _realDatabase = new RealDataBase(options);
            _realUserRepository = new RealUserRepository(_realDatabase);

            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            }).CreateLogger<GetAllUsersQueryHandler>();

            
        }

        [SetUp]
        public void SetUp()
        {
            _handler = new GetAllUsersQueryHandler(_realUserRepository, _logger);
            _realDatabase.Database.EnsureDeleted();
            _realDatabase.Database.EnsureCreated();

            _realDatabase.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            });
            _realDatabase.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "user2",
                Email = "user2@example.com",
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
        public async Task Handle_ShouldReturnAllUsers()
        {
            // Arrange
            var query = new GetAllUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Users retrieved successfully."));
            Assert.That(result.Result.Count, Is.EqualTo(2));
            Assert.That(result.Result.Any(u => u.Username == "user1"));
            Assert.That(result.Result.Any(u => u.Username == "user2"));
        }

        public void Dispose()
        {
            _realDatabase?.Dispose();
        }
    }
}
