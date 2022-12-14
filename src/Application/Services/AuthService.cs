using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly TokenSettings _tokenSettings;
    private readonly IApplicationDbContext _context;

    public AuthService(IOptions<TokenSettings> tokenSettings, IApplicationDbContext context)
    {
        _tokenSettings = tokenSettings.Value
            ?? throw new ArgumentNullException(nameof(tokenSettings));

        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string CreatePasswordHash(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(text: password, hash: hash);

    public async Task<AuthResult> LoginAsync(LoginDto loginDto)
    {
        var result = new AuthResult();
        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);

        if (user is null)
        {
            result.Result = AuthResults.NotFoundUser;
            return result;
        }
        if (!VerifyPassword(loginDto.Password, user!.Password))
        {
            result.Result = AuthResults.NotValidPassword;
            return result;
        }

        var accessToken = CreateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync();

        result.AccessToken = accessToken;
        result.RefreshToken = refreshToken;
        result.Result = AuthResults.Success;
        return result;
    }

    private string CreateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("id", user.Id.ToString())
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

    public async Task LogoutAsync(Guid userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId);

        if (user is not null)
        {
            user.RefreshToken = null;
            await _context.SaveChangesAsync();
        }
    }
}