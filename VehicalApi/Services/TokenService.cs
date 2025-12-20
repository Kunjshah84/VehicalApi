using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VehicalApi.Entity;
using System.Security.Cryptography;
using VehicalApi.Exceptions;

public interface ITokenService
{
    string CreateAccessToken(User user);
    (string token, DateTimeOffset expiry) CreateRefreshToken();
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config) => _config = config;

    public string CreateAccessToken(User user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("fullName", user.FullName)
        };

        string temp=jwtSection["AccessTokenExpirationMinutes"]!;
        if (string.IsNullOrEmpty(temp))
            throw new ConfigurationException(
            "JWT AccessTokenExpirationMinutes is not configured."
        );


        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(temp)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string token, DateTimeOffset expiry) CreateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);
        string temp=_config["Jwt:RefreshTokenExpirationDays"]!;
        if (string.IsNullOrEmpty(temp))
        throw new ConfigurationException(
            "JWT RefreshTokenExpirationDays is not configured."
        );
        var expiry = DateTimeOffset.UtcNow.AddDays(int.Parse(temp));
        return (token, expiry);
    }
}
