using Application.Interfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    public string CreatePasswordHash(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(text: password, hash: hash);
}