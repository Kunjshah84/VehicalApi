using Moq;
using VehicalApi.Domain.Auth.Services;
using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.Services.Interfaces;
using VehicalApi.Entity;
using VehicalApi.Exceptions;
using VehicalApi.DTOs;

namespace VehicalApi.Tests.Domain.Auth
{
    public class AuthDomainServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHashService> _passwordHashMock;
        private readonly AuthDomainService _sut;

        public AuthDomainServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHashMock = new Mock<IPasswordHashService>();

            _sut = new AuthDomainService(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _passwordHashMock.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrowBadRequestException()
        {
            var dto = new RegisterDto
            {
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Password@123",
                Number = "9999999999"
            };

            _userRepoMock
                .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BadRequestException>(
                () => _sut.RegisterAsync(dto)
            );
        }

        [Fact]
        public async Task RegisterAsync_WhenValid_ShouldCreateUserAndReturnAuthResponse()
        {
            var dto = new RegisterDto
            {
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Password@123",
                Number = "9999999999"
            };

            _userRepoMock
                .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _passwordHashMock
                .Setup(p => p.HashPassword(It.IsAny<User>(), dto.Password))
                .Returns("HASHED_PASSWORD");

            _tokenServiceMock
                .Setup(t => t.CreateRefreshToken())
                .Returns(("REFRESH_TOKEN", DateTimeOffset.UtcNow.AddDays(7)));

            _tokenServiceMock
                .Setup(t => t.CreateAccessToken(It.IsAny<User>()))
                .Returns("ACCESS_TOKEN");

            var result = await _sut.RegisterAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("ACCESS_TOKEN", result.Token);
            Assert.Equal("REFRESH_TOKEN", result.RefreshToken);

            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorizedException()
        {
            var dto = new LoginDto
            {
                Email = "missing@test.com",
                Password = "Password@123"
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _sut.LoginAsync(dto)
            );
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordInvalid_ShouldThrowUnauthorizedException()
        {
            var user = new User
            {
                Email = "test@test.com",
                PasswordHash = "HASH"
            };

            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _passwordHashMock
                .Setup(p => p.VerifyPassword(user, user.PasswordHash, dto.Password))
                .Returns(false);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _sut.LoginAsync(dto)
            );
        }

        [Fact]
        public async Task LoginAsync_WhenValid_ShouldReturnAuthResponse()
        {
            var user = new User
            {
                UserId = 1,
                Email = "test@test.com",
                PasswordHash = "HASH",
                FullName = "Test",
                Role = "User"
            };

            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "Password@123"
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _passwordHashMock
                .Setup(p => p.VerifyPassword(user, user.PasswordHash, dto.Password))
                .Returns(true);

            _tokenServiceMock
                .Setup(t => t.CreateRefreshToken())
                .Returns(("REFRESH", DateTimeOffset.UtcNow.AddDays(7)));

            _tokenServiceMock
                .Setup(t => t.CreateAccessToken(user))
                .Returns("ACCESS");

            var result = await _sut.LoginAsync(dto);

            Assert.Equal("ACCESS", result.Token);
            Assert.Equal("REFRESH", result.RefreshToken);

            _userRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshAsync_WhenTokenInvalid_ShouldThrowUnauthorizedException()
        {
            _userRepoMock
                .Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _sut.RefreshAsync("INVALID_TOKEN")
            );
        }

        [Fact]
        public async Task RefreshAsync_WhenValid_ShouldRotateTokenAndReturnAuthResponse()
        {
            var user = new User
            {
                UserId = 1,
                Email = "test@test.com",
                FullName = "Test User",
                Role = "User",
                RefreshToken = "OLD_TOKEN"
            };

            _userRepoMock
                .Setup(r => r.GetByRefreshTokenAsync("OLD_TOKEN"))
                .ReturnsAsync(user);

            _tokenServiceMock
                .Setup(t => t.CreateRefreshToken())
                .Returns(("NEW_REFRESH", DateTimeOffset.UtcNow.AddDays(7)));

            _tokenServiceMock
                .Setup(t => t.CreateAccessToken(user))
                .Returns("NEW_ACCESS");

            var result = await _sut.RefreshAsync("OLD_TOKEN");

            Assert.Equal("NEW_ACCESS", result.Token);
            Assert.Equal("NEW_REFRESH", result.RefreshToken);

            _userRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task LogoutAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            var dto = new LogoutDto
            {
                Email = "missing@test.com"
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.LogoutAsync(dto)
            );
        }

        [Fact]
        public async Task LogoutAsync_WhenValid_ShouldClearRefreshTokenAndSave()
        {
            var user = new User
            {
                Email = "test@test.com",
                RefreshToken = "REFRESH",
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            var dto = new LogoutDto
            {
                Email = "test@test.com"
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            await _sut.LogoutAsync(dto);

            Assert.Null(user.RefreshToken);
            Assert.Null(user.RefreshTokenExpiry);

            _userRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
