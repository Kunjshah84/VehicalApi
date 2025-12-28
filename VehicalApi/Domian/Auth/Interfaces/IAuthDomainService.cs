using VehicalApi.DTOs;
using VehicalApi.DTOs.Auth;

namespace VehicalApi.Domain.Auth.Interfaces
{
    public interface IAuthDomainService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(LogoutDto dto);
    }
}
