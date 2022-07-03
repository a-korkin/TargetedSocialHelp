using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "cd_users",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    c_user_name = table.Column<string>(type: "text", nullable: false),
                    c_last_name = table.Column<string>(type: "text", nullable: false),
                    c_first_name = table.Column<string>(type: "text", nullable: false),
                    c_middle_name = table.Column<string>(type: "text", nullable: true),
                    c_password = table.Column<string>(type: "text", nullable: false),
                    b_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_users", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "cd_users",
                columns: new[] { "id", "c_first_name", "b_deleted", "c_last_name", "c_middle_name", "c_password", "c_user_name" },
                values: new object[] { new Guid("d3f67070-a5e9-4891-bd64-be10f81888d9"), "Администратор", false, "Администратор", null, "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J.", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cd_users",
                schema: "admin");
        }
    }
}
