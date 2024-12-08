using System.Collections.Generic;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Infrastructure.DataBase
{
    public class FakeDatabase
    {
        public List<Book> Books { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public FakeDatabase()
        {
            // Add pre-existing authors
            Authors.AddRange(new List<Author>
        {
            new Author { Id = Guid.NewGuid(), Name = "J.K. Rowling" },
            new Author { Id = Guid.NewGuid(), Name = "George R.R. Martin" },
            new Author { Id = Guid.NewGuid(), Name = "J.R.R. Tolkien" }
        });

            // Add pre-existing books
            Books.AddRange(new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Harry Potter and the Sorcerer's Stone", AuthorId = Authors[0].Id, Year = 1997 },
            new Book { Id = Guid.NewGuid(), Title = "A Game of Thrones", AuthorId = Authors[1].Id, Year = 1996 },
            new Book { Id = Guid.NewGuid(), Title = "The Hobbit", AuthorId = Authors[2].Id, Year = 1937 }
        });
            Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                CreatedAt = DateTime.UtcNow
            });
        }
    }

}