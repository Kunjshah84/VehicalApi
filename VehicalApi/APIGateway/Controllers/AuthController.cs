using Microsoft.AspNetCore.Mvc;
using VehicalApi.DTOs;
using VehicalApi.Exceptions;
using VehicalApi.Business.Auth.Interfaces;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    // For testing poroposr
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("AUTH CONTROLLER HIT");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiry
        );

        return Ok(new
        {
            token = result.Token,
            user = result.User
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        Console.WriteLine("in the route of the login");
        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiry
        );

        return Ok(new
        {
            token = result.Token,
            user = result.User
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            throw new UnauthorizedException("Invalid refresh token");

        var result = await _authService.RefreshAsync(refreshToken);

        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiry
        );
        return Ok(new
        {
            token = result.Token
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutDto dto)
    {
        await _authService.LogoutAsync(dto);

        Response.Cookies.Delete("refreshToken");

        return NoContent();
    }

    private void SetRefreshTokenCookie(string token, DateTimeOffset expiry)
    {
        Console.WriteLine("In the refresh token like after gebnerating the two tokens");
        Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false, 
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = expiry.UtcDateTime
            }
        );
    }
}
