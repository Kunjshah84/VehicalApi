using VehicalApi.Entity;

namespace VehicalApi.Domain.Auth.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task AddAsync(User user);
        Task SaveAsync();

        Task<User?> GetByIdAsync(int userId);

    }
}
