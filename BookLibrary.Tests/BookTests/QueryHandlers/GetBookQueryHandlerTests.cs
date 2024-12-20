﻿using NUnit.Framework;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Books.Queries.GetBook;
using BookLibrary.Infrastructure.DataBase;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Tests.QueryHandlers
{
    [TestFixture]
    public class GetBookQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private FakeBookRepository _fakeRepository;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _fakeRepository = new FakeBookRepository(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnCorrectBook()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeRepository);
            var book = new Book("Test Book") { Id = Guid.NewGuid() };
            _fakeDatabase.Books.Add(book);

            var query = new GetBookQuery(book.Id);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Book retrieved successfully.")); 
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.Title, Is.EqualTo("Test Book"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
        {
            // Arrange
            var handler = new GetBookQueryHandler(_fakeRepository);
            var query = new GetBookQuery(Guid.NewGuid());

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.That(result.Success, Is.False); 
            Assert.That(result.Message, Is.EqualTo("Book not found.")); 
            Assert.That(result.Result, Is.Null); 
        }
    }
}
