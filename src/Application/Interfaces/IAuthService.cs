using Application.Models.Dtos.Admin;
using Application.Models.Helpers;

namespace Application.Interfaces;

public interface IAuthService
{
    string CreatePasswordHash(string password);
    bool VerifyPassword(string password, string hash);
    Task<AuthResult> LoginAsync(LoginDto loginDto);
    Task LogoutAsync(Guid userId);
}