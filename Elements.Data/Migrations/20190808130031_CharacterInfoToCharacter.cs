using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class CharacterInfoToCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterInfoId",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CharacterInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: false),
                    LocationX = table.Column<double>(nullable: false),
                    LocationY = table.Column<double>(nullable: false),
                    LocationZ = table.Column<double>(nullable: false),
                    RotationX = table.Column<double>(nullable: false),
                    RotationY = table.Column<double>(nullable: false),
                    RotationZ = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterInfos_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInfos_CharacterId",
                table: "CharacterInfos",
                column: "CharacterId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterInfos");

            migrationBuilder.DropColumn(
                name: "CharacterInfoId",
                table: "Characters");
        }
    }
}
