using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.Entity;

namespace VehicalApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _db;

        public UserRepository(MyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            Console.WriteLine("Insidet the email finding");
            return await _db.Users.AnyAsync(u => u.Email.ToLower() == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.SingleOrDefaultAsync(
                u => u.Email.ToLower() == email
            );
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _db.Users.SingleOrDefaultAsync(u =>
                u.RefreshToken == refreshToken &&
                u.RefreshTokenExpiry > DateTime.UtcNow
            );
        }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
