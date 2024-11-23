using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Infrastructure.Repositories
{
    public class FakeBookRepository : IBookRepository
    {
        private readonly FakeDatabase _fakeDatabase;

        public FakeBookRepository(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task AddAsync(Book book)
        {
            _fakeDatabase.Books.Add(book);
            return Task.CompletedTask;
        }

        public Task<Book> GetByIdAsync(Guid id) =>
            Task.FromResult(_fakeDatabase.Books.FirstOrDefault(book => book.Id == id));

        public Task<IEnumerable<Book>> GetAllAsync() =>
            Task.FromResult(_fakeDatabase.Books.AsEnumerable());

        public Task DeleteAsync(Guid id)
        {
            var bookToDelete = _fakeDatabase.Books.FirstOrDefault(book => book.Id == id);
            if (bookToDelete != null) _fakeDatabase.Books.Remove(bookToDelete);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Book book)
        {
            var bookToUpdate = _fakeDatabase.Books.FirstOrDefault(existingBook => existingBook.Id == book.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.AuthorId = book.AuthorId;
                bookToUpdate.Year = book.Year;
            }
            return Task.CompletedTask;
        }
    }
}
