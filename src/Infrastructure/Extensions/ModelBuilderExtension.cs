using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder builder)
    {
        builder.Entity<User>().HasData
        (
            new User
            {
                Id = Guid.Parse("d3f67070-a5e9-4891-bd64-be10f81888d9"),
                UserName = "admin",
                LastName = "Администратор",
                FirstName = "Администратор",
                Password = "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J."
            }
        );
    }
}