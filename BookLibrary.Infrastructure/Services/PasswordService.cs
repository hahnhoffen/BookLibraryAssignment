using BCrypt.Net;
using BookLibrary.Domain.Interface;

namespace BookLibrary.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var isMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                Console.WriteLine($"Verifying: {password} with {hashedPassword} -> {isMatch}");
                return isMatch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying password: {ex.Message}");
                throw;
            }
        }

    }
}

