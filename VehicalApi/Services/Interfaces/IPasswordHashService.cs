using VehicalApi.Entity;

namespace VehicalApi.Services.Interfaces
{
    public interface IPasswordHashService
    {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string hashedPassword, string password);
    }
}
