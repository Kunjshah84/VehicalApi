using System.Security.Claims;
using VehicalApi.Business.Auth.Interfaces;
using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.DTOs;
using VehicalApi.DTOs.Auth;

namespace VehicalApi.Business.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDomainService _domain;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(IAuthDomainService domain , IHttpContextAccessor httpContextAccessor)
        {
            _domain = domain;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            return await _domain.GetUserByIdAsync(userId);
        }
    }
}
