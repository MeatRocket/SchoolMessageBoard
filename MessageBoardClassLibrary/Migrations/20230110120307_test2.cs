using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicMedia_Template_TemplateId",
                table: "DynamicMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPosts_Template_TemplateId",
                table: "DynamicPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Template",
                table: "Template");

            migrationBuilder.RenameTable(
                name: "Template",
                newName: "DynamicTemplates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DynamicTemplates",
                table: "DynamicTemplates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicMedia_DynamicTemplates_TemplateId",
                table: "DynamicMedia",
                column: "TemplateId",
                principalTable: "DynamicTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPosts_DynamicTemplates_TemplateId",
                table: "DynamicPosts",
                column: "TemplateId",
                principalTable: "DynamicTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicMedia_DynamicTemplates_TemplateId",
                table: "DynamicMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPosts_DynamicTemplates_TemplateId",
                table: "DynamicPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DynamicTemplates",
                table: "DynamicTemplates");

            migrationBuilder.RenameTable(
                name: "DynamicTemplates",
                newName: "Template");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Template",
                table: "Template",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicMedia_Template_TemplateId",
                table: "DynamicMedia",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPosts_Template_TemplateId",
                table: "DynamicPosts",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
