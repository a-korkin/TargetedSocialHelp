using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public AuthController(IApplicationDbContext context, IAuthService authService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("api/login")]
    public async Task<ActionResult<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);

        if (user is null)
            return BadRequest();

        if (!_authService.VerifyPassword(loginDto.Password, user.Password))
            return Unauthorized();

        return _authService.GetAuthToken(user);
    }
}