using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Tests.Attributes;

namespace WebApi.Tests.Controllers;

public class AuthControllerShould
{
    private readonly AuthController _authController;

    public AuthControllerShould()
    {
        Mock<IApplicationDbContext> mockDbContext = MockHelper.CreateDbContext();
        IAuthService authService = MockHelper.CreateAuthService(mockDbContext);

        ControllerContext controllerContext = new()
        {
            HttpContext = new DefaultHttpContext()
        };
        _authController = new AuthController(authService)
        {
            ControllerContext = controllerContext
        };
    }

    [Theory]
    [LoginData]
    public async Task LogedIn(Type expectedAction, LoginDto loginDto)
    {
        // act 
        var result = await _authController.LoginAsync(loginDto);

        // assert   
        Assert.IsType(expectedAction, result?.Result);
    }

    [Fact]
    public async Task LogedOut()
    {
        // arrange
        MockHelper.AdminUser.RefreshToken = Guid.NewGuid().ToString();

        // act 
        await _authController.LogoutAsync();

        // assert
        Assert.Null(MockHelper.AdminUser.RefreshToken);
    }
}