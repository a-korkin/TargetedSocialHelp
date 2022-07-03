using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private const string REFRESH_TOKEN = "refresh_token";

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result.Result == AuthResults.NotFoundUser)
            return BadRequest();

        if (result.Result == AuthResults.NotValidPassword)
            return Unauthorized();

        Response.Cookies.Append(REFRESH_TOKEN, result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7)
        });

        return new TokenDto(result.AccessToken);
    }

    [HttpPost("logout")]
    public async Task LogoutAsync()
    {
        var userId = HttpContext.User.Claims
            .Where(c => c.Type == "id")
            .Select(u => u.Value)
            .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            await _authService.LogoutAsync(Guid.Parse(userId));
            Response.Cookies.Delete(REFRESH_TOKEN);
        }
    }
}