using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicalApi.Data;
using VehicalApi.DTOs;
using VehicalApi.Entity;
using VehicalApi.Exceptions;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly MyDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthController(MyDbContext db, ITokenService tokenService, IConfiguration config)
    {
        _db = db;
        _tokenService = tokenService;
        _config = config;
        _passwordHasher = new PasswordHasher<User>();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        Console.WriteLine("Register DTO Received:");
        var email = dto.Email.Trim().ToLower();
        if (await _db.Users.AnyAsync(u => u.Email.ToLower() == email))
            // return BadRequest(new { message = "Email is already registered." });
            throw new BadRequestException("Email is already registered.");

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Number = dto.Number.Trim(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        string refreshToken;
        DateTimeOffset expiry;
        (refreshToken, expiry) = _tokenService.CreateRefreshToken();

        // try
        // {
        //     (refreshToken, expiry) = _tokenService.CreateRefreshToken();
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, new { message = ex.Message });
        // }

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry.UtcDateTime;

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        string accessToken;

        accessToken = _tokenService.CreateAccessToken(user);
        // try
        // {
        //     accessToken = _tokenService.CreateAccessToken(user);
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, new { message = ex.Message });
        // }

        // var accessToken = _tokenService.CreateAccessToken(user);
        SetRefreshTokenCookie(refreshToken, expiry);
        return Ok(new
        {
            token = accessToken,
            user = new { user.UserId, user.FullName, user.Email, user.Number, user.Role }
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)  throw new UnauthorizedException("Please Register First");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)  
            throw new UnauthorizedException("Wrong Password");
        
        string accessToken;

        accessToken = _tokenService.CreateAccessToken(user);
        // try
        // {
        //     accessToken = _tokenService.CreateAccessToken(user);
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, new { message = ex.Message });
        // }

        string refreshToken;
        DateTimeOffset expiry;

        (refreshToken, expiry) = _tokenService.CreateRefreshToken();
        // try
        // {
        //     (refreshToken, expiry) = _tokenService.CreateRefreshToken();
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, new { message = ex.Message });
        // }

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry.UtcDateTime;

        await _db.SaveChangesAsync();
        SetRefreshTokenCookie(refreshToken, expiry);

        return Ok(new { token = accessToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshDto dto)
    {
        // var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        // Console.WriteLine("PRINCIPAL CLAIMS:");
        // foreach (var claim in principal.Claims)
        // {
        //     Console.WriteLine($"CLAIM => {claim.Type} : {claim.Value}");
        // }
        // var temp = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var refreshToken = Request.Cookies["refreshToken"];
        
        if(string.IsNullOrEmpty(refreshToken)){
            // Console.WriteLine("The refresh token is null");
            // return Unauthorized("Invalid refresh token");
            throw new UnauthorizedException("Invalid refresh token");
        }
        // var userId = int.Parse(temp);

        // var user = await _db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        var user = await _db.Users.SingleOrDefaultAsync(u =>
            u.RefreshToken == refreshToken &&
            u.RefreshTokenExpiry > DateTime.UtcNow);
        if (user == null) throw new UnauthorizedException("Invalid refresh token");

        var newAccessToken = _tokenService.CreateAccessToken(user);
        var (newRefreshToken, newExpiry) = _tokenService.CreateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = newExpiry.UtcDateTime;

        await _db.SaveChangesAsync();
        SetRefreshTokenCookie(newRefreshToken, newExpiry);

        return Ok(new { token = newAccessToken});
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) throw new NotFoundException("User not found");

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Afgter understanding there is no need for the get principal method


    // private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    // {
    //     var jwtSection = _config.GetSection("Jwt");

    //     var tokenValidationParameters = new TokenValidationParameters
    //     {
    //         ValidateAudience = true,
    //         ValidateIssuer = true,
    //         ValidIssuer = jwtSection["Issuer"],
    //         ValidAudience = jwtSection["Audience"],
    //         ValidateIssuerSigningKey = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!)),
    //         ValidateLifetime = false
    //     };

    //     var handler = new JwtSecurityTokenHandler();
    //     var principal = handler.ValidateToken(token, tokenValidationParameters, out var securityToken);

    //     if (securityToken is not JwtSecurityToken jwtToken)
    //         throw new SecurityTokenException("Invalid token");

    //     return principal;
    // }

    // [HttpGet("tp")]
    // [Authorize]
    // public IActionResult Test()
    // {
    //     return Ok(new { message = "do not going to put the token in the authentication header..!" });
    // }

    private void SetRefreshTokenCookie(string token, DateTimeOffset expiry)
    {
        Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh",
                Expires = expiry.UtcDateTime
            }
        );
    }
}
