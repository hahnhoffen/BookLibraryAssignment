using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;

public class FakeAuthorRepository : IAuthorRepository
{
    private readonly FakeDatabase _fakeDatabase;

    public FakeAuthorRepository(FakeDatabase fakeDatabase)
    {
        _fakeDatabase = fakeDatabase;
    }

    public Task AddAsync(Author author)
    {
        var existingAuthor = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == author.Id);
        if (existingAuthor != null)
        {
            // Update existing author
            existingAuthor.Name = author.Name;
        }
        else
        {
            // Add new author if not found
            _fakeDatabase.Authors.Add(author);
        }
        return Task.CompletedTask;
    }

    public Task<Author> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_fakeDatabase.Authors.FirstOrDefault(a => a.Id == id));
    }

    public Task<IEnumerable<Author>> GetAllAsync()
    {
        return Task.FromResult(_fakeDatabase.Authors.AsEnumerable());
    }

    public Task DeleteAsync(Guid id)
    {
        var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == id);
        if (author != null)
        {
            _fakeDatabase.Authors.Remove(author);
        }
        return Task.CompletedTask;
    }
}

