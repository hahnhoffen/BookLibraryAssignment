using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Infrastructure.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly FakeDatabase _fakeDatabase;

        public FakeUserRepository(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public async Task AddAsync(User user)
        {
            if (_fakeDatabase.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                return;
            }

            _fakeDatabase.Users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = _fakeDatabase.Users.FirstOrDefault(u => u.Username == username);
            return await Task.FromResult(user);
        }

        public async Task<bool> IsUsernameOrEmailTakenAsync(string username, string email)
        {
            var isTaken = _fakeDatabase.Users.Any(u => u.Username == username || u.Email == email);
            return await Task.FromResult(isTaken);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.FromResult(_fakeDatabase.Users);
        }
        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await Task.FromResult(_fakeDatabase.Users.FirstOrDefault(u => u.Id == userId));
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = _fakeDatabase.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _fakeDatabase.Users.Remove(user);
            }
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(User user)
        {
            var existingUser = _fakeDatabase.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
            }
            await Task.CompletedTask;
        }
    }
}
