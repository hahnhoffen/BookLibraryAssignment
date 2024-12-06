using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RealAuthorRepository : IAuthorRepository
{
    private readonly RealDataBase _dbContext;

    public RealAuthorRepository(RealDataBase dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Author> GetByIdAsync(Guid id)
    {
        return await _dbContext.Authors
            .Include(a => a.Books) // Include related books
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Author author)
    {
        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _dbContext.Authors
            .Include(a => a.Books) // Include related books
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var author = await _dbContext.Authors.FindAsync(id);
        if (author != null)
        {
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}
