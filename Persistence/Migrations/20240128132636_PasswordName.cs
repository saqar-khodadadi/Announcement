using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class PasswordName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password_PasswordSalt",
                schema: "SSO",
                table: "User",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Password_PasswordHash",
                schema: "SSO",
                table: "User",
                newName: "PasswordHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                schema: "SSO",
                table: "User",
                newName: "Password_PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "SSO",
                table: "User",
                newName: "Password_PasswordHash");
        }
    }
}
