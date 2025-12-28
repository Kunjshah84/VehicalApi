using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.DTOs;
using VehicalApi.DTOs.Auth;
using VehicalApi.Entity;
using VehicalApi.Exceptions;
using VehicalApi.Services.Interfaces;

namespace VehicalApi.Domain.Auth.Services
{
    public class AuthDomainService : IAuthDomainService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHashService _passwordHashService;

        public AuthDomainService(
            IUserRepository userRepo,
            ITokenService tokenService,
            IPasswordHashService passwordHashService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            if (await _userRepo.ExistsByEmailAsync(email))
                throw new BadRequestException("Email is already registered.");

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                Number = dto.Number.Trim(),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _passwordHashService.HashPassword(user, dto.Password);

            var (refreshToken, expiry) = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry.UtcDateTime;

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

            return BuildResponse(user, refreshToken, expiry);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var user = await _userRepo.GetByEmailAsync(email)
                ?? throw new UnauthorizedException("Please Register First");

            if (!_passwordHashService.VerifyPassword(
                user, user.PasswordHash, dto.Password))
                throw new UnauthorizedException("Wrong Password");

            var (refreshToken, expiry) = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry.UtcDateTime;

            await _userRepo.SaveAsync();

            return BuildResponse(user, refreshToken, expiry);
        }

        public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken)
                ?? throw new UnauthorizedException("Invalid refresh token");

            var (newRefreshToken, newExpiry) =
                _tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = newExpiry.UtcDateTime;

            await _userRepo.SaveAsync();

            return BuildResponse(user, newRefreshToken, newExpiry);
        }

        public async Task LogoutAsync(LogoutDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email)
                ?? throw new NotFoundException("User not found");

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _userRepo.SaveAsync();
        }

        private AuthResponseDto BuildResponse(
            User user, string refreshToken, DateTimeOffset expiry)
        {
            return new AuthResponseDto
            {
                Token = _tokenService.CreateAccessToken(user),
                RefreshToken = refreshToken,
                RefreshTokenExpiry = expiry,
                User = new AuthUserResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Number = user.Number,
                    Role = user.Role
                }
            };
        }
    }
}
