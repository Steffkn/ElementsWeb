using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class Character_InventoryType_BodyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BodyType",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventorySize",
                table: "Characters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventorySize",
                table: "Characters");
        }
    }
}
