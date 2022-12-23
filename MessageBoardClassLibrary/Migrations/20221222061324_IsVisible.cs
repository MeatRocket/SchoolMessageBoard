using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class IsVisible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Posts");
        }
    }
}
