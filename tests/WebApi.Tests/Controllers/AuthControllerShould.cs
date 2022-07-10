using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Application.Services;
using Application.Tests.Helpers;
using Domain.Entities.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using WebApi.Controllers;
using WebApi.Tests.Attributes;

namespace WebApi.Tests.Controllers;

public class AuthControllerShould
{
    private readonly AuthController _authController;

    public AuthControllerShould()
    {
        Mock<IApplicationDbContext> mockDbContext = MockDbContext.Create();
        IAuthService authService = MockAuthService.Create(mockDbContext);

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
}