using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Infrastructure.Repositories
{
    public class FakeAuthorRepository : IAuthorRepository
    {
        private readonly FakeDatabase _database;

        public FakeAuthorRepository(FakeDatabase database)
        {
            _database = database;
        }

        public Task AddAsync(Author author)
        {
            _database.Authors.Add(author);
            return Task.CompletedTask;
        }

        public Task<Author> GetByIdAsync(Guid id) =>
            Task.FromResult(_database.Authors.FirstOrDefault(author => author.Id == id));

        public Task<IEnumerable<Author>> GetAllAsync() =>
            Task.FromResult(_database.Authors.AsEnumerable());

        public Task DeleteAsync(Guid id)
        {
            var authorToDelete = _database.Authors.FirstOrDefault(author => author.Id == id);
            if (authorToDelete != null) _database.Authors.Remove(authorToDelete);
            return Task.CompletedTask;
        }
    }
}
