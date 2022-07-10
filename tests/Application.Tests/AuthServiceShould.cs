using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Application.Services;
using Domain.Entities.Admin;
using Microsoft.Extensions.Options;
using Moq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests;

public class AuthServiceShould
{
    private const string PASSWORD = "admin";
    private readonly Guid USER_ID = Guid.Parse("97159bc1-2ffd-421e-acb2-a07d869526c6");
    private readonly IAuthService _authService;
    private readonly Mock<IApplicationDbContext> _context;

    public AuthServiceShould()
    {
        var token = new TokenSettings
        {
            SecretKey = "super_secret_key_strong",
            Audience = "SecretAudience",
            Issuer = "SecretIssuer"
        };

        IOptions<TokenSettings> tokenSettings = Options.Create<TokenSettings>(token);
        _context = new();
        List<User> users = new()
        {
            new()
            {
                Id = USER_ID,
                UserName = "admin",
                Password = "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J."
            }
        };

        var mock = users.AsQueryable().BuildMockDbSet();
        _context.Setup(x => x.Set<User>()).Returns(mock.Object);
        _authService = new Mock<AuthService>(tokenSettings, _context.Object).Object;
    }

    [Fact]
    public void PasswordHashed()
    {
        // act 
        string hashedPassword = _authService.CreatePasswordHash(PASSWORD);

        // assert        
        Assert.NotEqual(PASSWORD, hashedPassword);
        Assert.Contains("$", hashedPassword);
    }

    [Fact]
    public void PasswordVerified()
    {
        // arrange
        string hashedPassword = _authService.CreatePasswordHash(PASSWORD);

        // act 
        bool passwordVerified = _authService.VerifyPassword(PASSWORD, hashedPassword);

        // assert
        Assert.True(passwordVerified);
    }

    [Fact]
    public async Task LogedIn()
    {
        // arrange
        LoginDto loginDto = new("admin", PASSWORD);

        // act 
        var authResult = await _authService.LoginAsync(loginDto);

        // assert
        Assert.True(authResult.Result == AuthResults.Success);
        Assert.NotEmpty(authResult.AccessToken);
        Assert.NotEmpty(authResult.RefreshToken);
    }

    [Fact]
    public async Task LogedOut()
    {
        // arrange
        var user = await _context.Object.Set<User>()
            .SingleOrDefaultAsync(u => u.Id == USER_ID);

        // act 
        await _authService.LogoutAsync(USER_ID);

        // assert
        Assert.NotNull(user);
        Assert.Null(user!.RefreshToken);
    }
}