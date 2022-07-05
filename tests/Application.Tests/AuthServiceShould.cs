using System.Threading;
using Application.Interfaces;
using Application.Models.Helpers;
using Application.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests;

public class AuthServiceShould
{
    private const string PASSWORD = "my_secret_password";
    private readonly Mock<AuthService> _authServiceMock;
    
    public AuthServiceShould()
    {
        IOptions<TokenSettings> tokenSettings = Options.Create<TokenSettings>(new TokenSettings());
        IApplicationDbContext context = new Mock<IApplicationDbContext>().Object;
        _authServiceMock = new Mock<AuthService>(tokenSettings, context);
    }

    [Fact]
    public void PasswordHashed()
    {
        // arrange
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

        // act 
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(PASSWORD, salt);

        // assert        
        Assert.NotEqual(PASSWORD, hashedPassword);
        Assert.Contains("$", hashedPassword);
    }

    [Fact]
    public void PasswordVerified()
    {
        // arrange
        string hashedPassword = _authServiceMock.Object.CreatePasswordHash(PASSWORD);

        // act 
        bool passwordVerified = _authServiceMock.Object.VerifyPassword(PASSWORD, hashedPassword);
        Console.WriteLine($"dfdsf: {hashedPassword}");

        // assert
        Assert.True(passwordVerified);
    }
}