using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class AddCharacterInventories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterInventories",
                columns: table => new
                {
                    CharacterId = table.Column<int>(nullable: false),
                    Slot = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInventories", x => new { x.CharacterId, x.Slot });
                    table.ForeignKey(
                        name: "FK_CharacterInventories_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharacterInventories_InteractableItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InteractableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInventories_ItemId",
                table: "CharacterInventories",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterInventories");
        }
    }
}
