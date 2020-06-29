using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class CompatibleLazyLoading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCover",
                table: "ImagePlaces",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ImageAppUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IdOwner = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageAppUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageAppUser_AspNetUsers_IdOwner",
                        column: x => x.IdOwner,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageAppUser_IdOwner",
                table: "ImageAppUser",
                column: "IdOwner",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageAppUser");

            migrationBuilder.DropColumn(
                name: "IsCover",
                table: "ImagePlaces");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Destinations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
