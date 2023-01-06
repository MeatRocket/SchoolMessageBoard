using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class TemplateDetails2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PopertyValue",
                table: "TemplateDetails",
                type: "nvarchar(max)",
                maxLength: 2000000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PopertyValue",
                table: "TemplateDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2000000);
        }
    }
}
