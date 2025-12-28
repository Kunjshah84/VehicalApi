using VehicalApi.Business.Auth.Interfaces;
using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.DTOs;
using VehicalApi.DTOs.Auth;

namespace VehicalApi.Business.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDomainService _domain;

        public AuthService(IAuthDomainService domain)
        {
            _domain = domain;
        }

        public Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            return _domain.RegisterAsync(dto);
        }
            
        public Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            return _domain.LoginAsync(dto);
        } 

        public Task<AuthResponseDto> RefreshAsync(string refreshToken)
        {
            return _domain.RefreshAsync(refreshToken);
        }
        public Task LogoutAsync(LogoutDto dto)
        {
            return _domain.LogoutAsync(dto);
        }
    }
}
