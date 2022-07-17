namespace Application.Models.Dtos.Admin;

public record UserInDto(
    string UserName,
    string LastName,
    string FirstName,
    string? MiddleName);

public record UserOutDto(
    Guid Id,
    string UserName,
    string LastName,
    string FirstName,
    string? MiddleName);