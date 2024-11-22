using BookLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Interface
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book> GetByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Book book);
    }
}
