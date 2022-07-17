namespace Application.Models.Dtos.Admin;

public record UserDto(
    Guid Id,
    string UserName,
    string LastName,
    string FirstName,
    string? MiddleName);