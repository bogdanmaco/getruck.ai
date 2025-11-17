using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loadmaster_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserThemePreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDarkMode",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThemeColors",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDarkMode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ThemeColors",
                table: "Users");
        }
    }
}
