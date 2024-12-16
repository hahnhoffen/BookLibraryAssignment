using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Infrastructure.Repositories
{
    public class RealUserRepository : IUserRepository
    {
        private readonly RealDataBase _dataBase;

        public RealUserRepository(RealDataBase dataBase)
        {
            _dataBase = dataBase;
        }

        public async Task AddAsync(User user)
        {
            await _dataBase.Users.AddAsync(user);
            await _dataBase.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dataBase.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsUsernameOrEmailTakenAsync(string username, string email)
        {
            return await _dataBase.Users.AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dataBase.Users.FindAsync(userId);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _dataBase.Users.FindAsync(userId);
            if (user != null)
            {
                _dataBase.Users.Remove(user);
                await _dataBase.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dataBase.Users.ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _dataBase.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.PasswordHash = user.PasswordHash;
                await _dataBase.SaveChangesAsync();
            }
        }
    }
}


