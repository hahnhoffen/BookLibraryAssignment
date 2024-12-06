using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RealBookRepository : IBookRepository
{
    private readonly RealDataBase _dbContext;

    public RealBookRepository(RealDataBase dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Book book)
    {
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        return await _dbContext.Books.FindAsync(id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbContext.Books.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book != null)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Book book)
    {
        var existingBook = await _dbContext.Books.FindAsync(book.Id);
        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.AuthorId = book.AuthorId;
            existingBook.Year = book.Year;

            _dbContext.Books.Update(existingBook);
            await _dbContext.SaveChangesAsync();
        }
    }
}
