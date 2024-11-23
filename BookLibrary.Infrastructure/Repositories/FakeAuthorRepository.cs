using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Infrastructure.Repositories
{
    public class FakeAuthorRepository : IAuthorRepository
    {
        private readonly FakeDatabase _fakeDatabase;

        public FakeAuthorRepository(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task AddAsync(Author author)
        {
            _fakeDatabase.Authors.Add(author);
            return Task.CompletedTask;
        }

        public Task<Author> GetByIdAsync(Guid id) =>
            Task.FromResult(_fakeDatabase.Authors.FirstOrDefault(author => author.Id == id));

        public Task<IEnumerable<Author>> GetAllAsync() =>
            Task.FromResult(_fakeDatabase.Authors.AsEnumerable());

        public Task DeleteAsync(Guid id)
        {
            var authorToDelete = _fakeDatabase.Authors.FirstOrDefault(author => author.Id == id);
            if (authorToDelete != null) _fakeDatabase.Authors.Remove(authorToDelete);
            return Task.CompletedTask;
        }
    }
}
