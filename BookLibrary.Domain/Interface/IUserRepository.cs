using BookLibrary.Domain.Entities;

namespace BookLibrary.Domain.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid userId);
        Task<bool> IsUsernameOrEmailTakenAsync(string username, string email);
        Task DeleteAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(User user);
    }
}