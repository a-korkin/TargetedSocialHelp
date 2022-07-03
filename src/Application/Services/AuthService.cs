using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Domain.Entities.Admin;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly TokenSettings _tokenSettings;

    public AuthService(IOptions<TokenSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings.Value ?? throw new ArgumentNullException(nameof(tokenSettings));
    }

    public string CreatePasswordHash(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(text: password, hash: hash);

    public TokenDto GetAuthToken(User user)
    {
        return new TokenDto
        (
            CreateJwtToken(user.UserName)
        );
    }

    private string CreateJwtToken(string userName)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, userName)
        };

        var jwtToken = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}