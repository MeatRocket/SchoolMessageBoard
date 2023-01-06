using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullDynamicPostsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PopertyValue",
                table: "TemplateDetails",
                type: "nvarchar(max)",
                maxLength: 2000000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2000000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PopertyValue",
                table: "TemplateDetails",
                type: "nvarchar(max)",
                maxLength: 2000000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2000000,
                oldNullable: true);
        }
    }
}
