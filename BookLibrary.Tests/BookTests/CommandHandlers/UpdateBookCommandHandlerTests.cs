﻿using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Commands.UpdateBook;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class UpdateBookCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldUpdateBookInDatabase()
        {
            // Arrange
            var handler = new UpdateBookCommandHandler(_fakeDatabase);
            var book = new Book("Original Title") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var updatedBook = new Book("Updated Title")
            {
                Id = book.Id,
                AuthorId = Guid.NewGuid(),
                Year = 2024
            };
            var command = new UpdateBookCommand(updatedBook);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(1));
            Assert.That(_fakeDatabase.Books[0].Title, Is.EqualTo("Updated Title"));
            Assert.That(_fakeDatabase.Books[0].AuthorId, Is.EqualTo(updatedBook.AuthorId));
            Assert.That(_fakeDatabase.Books[0].Year, Is.EqualTo(2024));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookToUpdateDoesNotExist()
        {
            // Arrange
            var handler = new UpdateBookCommandHandler(_fakeDatabase);
            var updatedBook = new Book("Non-Existent Book") { Id = Guid.NewGuid() };
            var command = new UpdateBookCommand(updatedBook);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("Book not found."));
        }
    }
}
