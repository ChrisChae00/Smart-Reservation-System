using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DineReserve.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "FirstName", "LastName", "Email", "Password", "Role" },
                values: new object[] { "Admin", "User", "admin@dinereserve.com", "admin123!", "Admin" }
);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
