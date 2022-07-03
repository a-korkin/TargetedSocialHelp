using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;

namespace Domain.Entities.Admin;

[TableDescription(name: "cd_users", schema: "admin", ruName: "пользователи")]
public class User : BaseEntity
{
    [Column("c_user_name")]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Column("c_last_name")]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [Column("c_first_name")]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Column("c_middle_name")]
    public string? MiddleName { get; set; }

    [Column("c_password")]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Column("c_refresh_token")]
    public string? RefreshToken { get; set; }
}