using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedTemplateandModel2Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicMedia_DynamicTemplates_TemplateId",
                table: "DynamicMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPosts_DynamicTemplates_TemplateId",
                table: "DynamicPosts");

            migrationBuilder.DropIndex(
                name: "IX_DynamicPosts_TemplateId",
                table: "DynamicPosts");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "DynamicPosts");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateId",
                table: "DynamicMedia",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AddColumn<string>(
                name: "DynamicPostId",
                table: "DynamicMedia",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicMedia_DynamicPostId",
                table: "DynamicMedia",
                column: "DynamicPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicMedia_DynamicPosts_DynamicPostId",
                table: "DynamicMedia",
                column: "DynamicPostId",
                principalTable: "DynamicPosts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicMedia_DynamicTemplates_TemplateId",
                table: "DynamicMedia",
                column: "TemplateId",
                principalTable: "DynamicTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicMedia_DynamicPosts_DynamicPostId",
                table: "DynamicMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicMedia_DynamicTemplates_TemplateId",
                table: "DynamicMedia");

            migrationBuilder.DropIndex(
                name: "IX_DynamicMedia_DynamicPostId",
                table: "DynamicMedia");

            migrationBuilder.DropColumn(
                name: "DynamicPostId",
                table: "DynamicMedia");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                table: "DynamicPosts",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateId",
                table: "DynamicMedia",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPosts_TemplateId",
                table: "DynamicPosts",
                column: "TemplateId");

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
    }
}
