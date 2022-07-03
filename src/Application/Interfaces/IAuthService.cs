using Application.Models.Dtos.Admin;
using Domain.Entities.Admin;

namespace Application.Interfaces;

public interface IAuthService
{
    string CreatePasswordHash(string password);
    bool VerifyPassword(string password, string hash);
    TokenDto GetAuthToken(User user);
}