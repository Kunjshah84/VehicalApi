using Microsoft.AspNetCore.Identity;
using VehicalApi.Entity;
using VehicalApi.Services.Interfaces;

namespace VehicalApi.Services.Implementations
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
