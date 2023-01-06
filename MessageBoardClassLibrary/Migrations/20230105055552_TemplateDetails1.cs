using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class TemplateDetails1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PopertyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PopertyType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PopertyValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PopertySequence = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateDetails");
        }
    }
}
