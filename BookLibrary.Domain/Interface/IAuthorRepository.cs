using BookLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Interface
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(Guid id);
        Task AddAsync(Author author);
        Task<IEnumerable<Author>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
