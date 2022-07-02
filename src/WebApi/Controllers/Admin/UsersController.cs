using Application.Interfaces;
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
}