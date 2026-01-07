using VehicalApi.DTOs;
using VehicalApi.DTOs.Auth;

namespace VehicalApi.Business.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(LogoutDto dto);
        Task<UserDto> GetCurrentUserAsync();
    }
}
