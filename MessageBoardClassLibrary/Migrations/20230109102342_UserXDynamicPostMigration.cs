using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UserXDynamicPostMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TemplateDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDetails_UserId",
                table: "TemplateDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateDetails_Users_UserId",
                table: "TemplateDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDetails_Users_UserId",
                table: "TemplateDetails");

            migrationBuilder.DropIndex(
                name: "IX_TemplateDetails_UserId",
                table: "TemplateDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TemplateDetails");
        }
    }
}
