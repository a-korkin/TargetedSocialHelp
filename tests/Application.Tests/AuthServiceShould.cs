using Application.Interfaces;
using Application.Models.Helpers;
using Application.Services;
using Application.Test.Helpers;
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
        IApplicationDbContext context = new MockApplicationDbContext();
        _authService = new Mock<AuthService>(tokenSettings, context).Object;
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
}