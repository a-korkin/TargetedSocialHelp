using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Domain.Entities.Admin;
using Moq;
using Microsoft.EntityFrameworkCore;
using Application.Tests.Helpers;

namespace Application.Tests;

public class AuthServiceShould
{
    private const string PASSWORD = "admin";
    private readonly IAuthService _authService;
    private readonly Mock<IApplicationDbContext> _context;

    public AuthServiceShould()
    {
        _context = MockDbContext.Create();
        _authService = MockAuthService.Create(_context);
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
            .SingleOrDefaultAsync(u => u.Id == MockDbContext.AdminUser.Id);

        // act 
        await _authService.LogoutAsync(MockDbContext.AdminUser.Id);

        // assert
        Assert.NotNull(user);
        Assert.Null(user!.RefreshToken);
    }
}