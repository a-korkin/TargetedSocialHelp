using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Application.Services;
using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests;

public class AuthServiceShould
{
    private const string PASSWORD = "my_secret_password";
    private readonly IAuthService _authService;

    public AuthServiceShould()
    {
        IOptions<TokenSettings> tokenSettings = Options.Create<TokenSettings>(new TokenSettings());
        Mock<IApplicationDbContext> context = new();
        List<User> userData = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserName = "admin"
            }
        };
        Mock<DbSet<User>> users = new();
        users.Setup(u => u.AddRange(userData));
        context.Setup(c => c.Set<User>()).Returns(users.Object);
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
    public async void Logined()
    {
        // arrange
        LoginDto loginDto = new("admin", PASSWORD);

        // act 
        var authResult = await _authService.LoginAsync(loginDto);

        // assert
        Assert.True(authResult.Result == AuthResults.Success);
    }
}