using VehicalApi.Entity;

namespace VehicalApi.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        (string refreshToken, DateTimeOffset expiry) CreateRefreshToken();
    }
}
