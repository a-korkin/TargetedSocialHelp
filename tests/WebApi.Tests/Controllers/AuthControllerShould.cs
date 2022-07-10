using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Application.Services;
using Domain.Entities.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers;

public class AuthControllerShould
{
    private readonly AuthController _authController;

    public AuthControllerShould()
    {
        var token = new TokenSettings
        {
            SecretKey = "super_secret_key_strong",
            Audience = "SecretAudience",
            Issuer = "SecretIssuer"
        };

        IOptions<TokenSettings> tokenSettings = Options.Create<TokenSettings>(token);
        Mock<IApplicationDbContext> context = new();
        List<User> users = new()
        {
            new()
            {
                Id = Guid.Parse("97159bc1-2ffd-421e-acb2-a07d869526c6"),
                UserName = "admin",
                Password = "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J."
            }
        };

        var mock = users.AsQueryable().BuildMockDbSet();
        context.Setup(x => x.Set<User>()).Returns(mock.Object);
        // IAuthService authService = new Mock<AuthService>(tokenSettings, context.Object).Object;
        Mock<AuthService> authService = new Mock<AuthService>(tokenSettings, context.Object);

        _authController = new AuthController(authService.Object);
    }

    [Fact]
    public async Task LogedIn()
    {
        // arrange
        LoginDto loginDto = new("admin", "admin");

        // act 
        var result = await _authController.LoginAsync(loginDto);
        Console.WriteLine(result?.Result?.GetType());

        // assert   
        Assert.IsType<OkObjectResult>(result?.Result);
        Assert.True(true);
    }
}