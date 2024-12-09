using Microsoft.EntityFrameworkCore;
using BookLibrary.Domain.Entities;
using System;

namespace BookLibrary.Infrastructure.Data
{
    public class RealDataBase : DbContext
    {
        public RealDataBase(DbContextOptions<RealDataBase> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne()
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Seed data only if the 'Author' table is not already configured
            if (!modelBuilder.Model.GetEntityTypes().Any(e => e.Name == "Author"))
            {
                modelBuilder.Entity<Author>().HasData(
                    new Author { Id = Guid.Parse("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"), Name = "J.K. Rowling" },
                    new Author { Id = Guid.Parse("a362ed9e-46fd-40e8-b6cf-8a945993e25e"), Name = "George R.R. Martin" }
                );
            }

            if (!modelBuilder.Model.GetEntityTypes().Any(e => e.Name == "Book"))
            {
                modelBuilder.Entity<Book>().HasData(
                    new Book
                    {
                        Id = Guid.Parse("e36660d0-156a-4d5b-ac1c-f05941966d48"),
                        Title = "Harry Potter",
                        AuthorId = Guid.Parse("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"),
                        Year = 1997
                    },
                    new Book
                    {
                        Id = Guid.Parse("fe57c76f-a346-4556-aad9-635fd67081cc"),
                        Title = "Game of Thrones",
                        AuthorId = Guid.Parse("a362ed9e-46fd-40e8-b6cf-8a945993e25e"),
                        Year = 1996
                    }
                );
            }
        }
    }
}
