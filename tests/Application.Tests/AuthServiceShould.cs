using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Application.Services;
using Domain.Entities.Admin;
using Microsoft.Extensions.Options;
using Moq;
using MockQueryable.Moq;

namespace Application.Tests;

public class AuthServiceShould
{
    private const string PASSWORD = "my_secret_password";
    private readonly IAuthService _authService;

    public AuthServiceShould()
    {
        var token = new TokenSettings
        {
            SecretKey = "super_secret_key_strong",
            Audience = "SecretAudience",
            Issuer = "SecretIssuer"
        };

        IOptions<TokenSettings> tokenSettings = Options.Create<TokenSettings>(token);
        Mock<IApplicationDbContext> context = new();
        var users = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Password = "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J."
            }
        };

        var mock = users.AsQueryable().BuildMockDbSet();
        context.Setup(x => x.Set<User>()).Returns(mock.Object);
        _authService = new Mock<AuthService>(tokenSettings, context.Object).Object;
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
    public async void LogedIn()
    {
        // arrange
        LoginDto loginDto = new("admin", PASSWORD);

        // act 
        var authResult = await _authService.LoginAsync(loginDto);

        // assert
        Assert.True(authResult.Result == AuthResults.Success);
    }
}