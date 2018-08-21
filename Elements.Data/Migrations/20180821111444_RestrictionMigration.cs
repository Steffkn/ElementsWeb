using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elements.Data.Migrations
{
    public partial class RestrictionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ForumCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "ForumCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRestricted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestrictionEndDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "IsRestricted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RestrictionEndDate",
                table: "AspNetUsers");
        }
    }
}
