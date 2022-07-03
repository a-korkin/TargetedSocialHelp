using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IApplicationDbContext _context;

    public UsersController(IAuthService authService, IApplicationDbContext context)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<string> HelloWorld()
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == "admin");
        if (user is null)
            return "user not found";

        Console.WriteLine($"password ok: {_authService.VerifyPassword("admin", user.Password)}");
        return "hello world";
    }

    [HttpPost]
    public async Task<ActionResult<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);

        if (user is null) return NotFound();
        if (!_authService.VerifyPassword(loginDto.Password, user.Password)) return Unauthorized();

        var token = _authService.GetAuthToken(user);
        return Ok(token);
    }
}