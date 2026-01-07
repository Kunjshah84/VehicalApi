using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VehicalApi.Entity;
using VehicalApi.Services.Interfaces;

namespace VehicalApi.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateAccessToken(User user)
        {
            Console.WriteLine("The token is created for the user");
            var jwt = _config.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwt["AccessTokenExpirationMinutes"]!)
                ),
                signingCredentials: creds
            );

            Console.WriteLine("AC Token is created");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string refreshToken, DateTimeOffset expiry) CreateRefreshToken()
        {
            // Console.WriteLine("refresh token called/made");
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(randomBytes);

            var expiryDays = int.Parse(
                _config["Jwt:RefreshTokenExpirationDays"]!
            );
            Console.WriteLine("AC Token is created");
            return (refreshToken, DateTimeOffset.UtcNow.AddDays(expiryDays));
        }
    }
}
