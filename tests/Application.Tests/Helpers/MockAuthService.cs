using Application.Interfaces;
using Application.Models.Helpers;
using Application.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Helpers;

public static class MockAuthService
{
    public static AuthService Create(Mock<IApplicationDbContext> mockDbContext)
    {
        TokenSettings tokenSettings = new()
        {
            SecretKey = "super_secret_key_strong",
            Audience = "SecretAudience",
            Issuer = "SecretIssuer"
        };
        IOptions<TokenSettings> token = Options.Create<TokenSettings>(tokenSettings);
        return new Mock<AuthService>(token, mockDbContext.Object).Object;
    }
}