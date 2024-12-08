using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Users.Commands.AddUser;
using MediatR;
using BookLibrary.Application.Common;
using System.Threading.Tasks;
using BookLibrary.Infrastructure.Services;

namespace BookLibrary.Tests.UserIntegrationTests
{
    [TestFixture]
    public class AddUserIntegrationTests
    {
        private RealDataBase _realDatabase;
        private RealUserRepository _realUserRepository;
        private AddUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RealDataBase>()
                .UseSqlServer("Server=DESKTOP-FSSLAO9\\SQLEXPRESS;Database=BookLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .Options;

            _realDatabase = new RealDataBase(options);

            _realDatabase.Database.EnsureCreated();

            _realUserRepository = new RealUserRepository(_realDatabase);
            _handler = new AddUserCommandHandler(_realUserRepository, new PasswordService());
        }

        [TearDown]
        public void TearDown()
        {
            _realDatabase.Dispose();
        }

        [Test]
        public async Task Handle_ShouldAddUserToDatabase()
        {
            // Arrange
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "integrationuser",
                Email = "integrationuser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            var command = new AddUserCommand(newUser);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User added successfully."));
            Assert.That(_realDatabase.Users.Count(), Is.EqualTo(1));

            var userInDatabase = await _realDatabase.Users.FirstOrDefaultAsync(u => u.Username == "integrationuser");
            Assert.That(userInDatabase, Is.Not.Null);
            Assert.That(userInDatabase.Email, Is.EqualTo("integrationuser@example.com"));
        }
    }
}
