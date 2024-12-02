﻿using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Commands.DeleteBook;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Tests.CommandHandlers
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
        }

        [Test]
        public async Task Handle_ShouldDeleteBookFromDatabase()
        {
            // Arrange
            var handler = new DeleteBookCommandHandler(_fakeDatabase);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var command = new DeleteBookCommand(book.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(0));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookToDeleteDoesNotExist()
        {
            // Arrange
            var handler = new DeleteBookCommandHandler(_fakeDatabase);
            var command = new DeleteBookCommand(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
            Assert.That(ex.Message, Is.EqualTo("Book not found."));
        }
    }
}
