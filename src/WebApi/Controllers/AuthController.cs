using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("api/login")]
    public async Task<ActionResult<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result.Result == AuthResults.NotFoundUser)
            return BadRequest();

        if (result.Result == AuthResults.NotValidPassword)
            return Unauthorized();

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7)
        });

        return new TokenDto(result.AccessToken);
    }
}