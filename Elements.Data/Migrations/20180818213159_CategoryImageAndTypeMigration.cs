using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class CategoryImageAndTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "ForumCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "ForumCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "ForumCategories");
        }
    }
}
