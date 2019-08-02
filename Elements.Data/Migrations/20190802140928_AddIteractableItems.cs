using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class AddIteractableItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InteractableItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsStackable = table.Column<bool>(nullable: false),
                    MaxStackSize = table.Column<int>(nullable: false),
                    IsUsable = table.Column<bool>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Rarity = table.Column<int>(nullable: false),
                    EquipmentSlot = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractableItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InteractableItems");
        }
    }
}
