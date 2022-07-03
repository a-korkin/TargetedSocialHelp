using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
namespace Application.Models.Helpers;

public record AuthResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public AuthResults Result { get; set; }
}

public enum AuthResults
{
    Success,
    NotFoundUser,
    NotValidPassword
}
