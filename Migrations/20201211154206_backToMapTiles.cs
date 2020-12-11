using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class backToMapTiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tiles",
                table: "Tiles");

            migrationBuilder.RenameTable(
                name: "Tiles",
                newName: "MapTiles");

            migrationBuilder.RenameIndex(
                name: "IX_Tiles_IdTrip",
                table: "MapTiles",
                newName: "IX_MapTiles_IdTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapTiles",
                table: "MapTiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTiles_Trips_IdTrip",
                table: "MapTiles",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapTiles_Trips_IdTrip",
                table: "MapTiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapTiles",
                table: "MapTiles");

            migrationBuilder.RenameTable(
                name: "MapTiles",
                newName: "Tiles");

            migrationBuilder.RenameIndex(
                name: "IX_MapTiles_IdTrip",
                table: "Tiles",
                newName: "IX_Tiles_IdTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tiles",
                table: "Tiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
