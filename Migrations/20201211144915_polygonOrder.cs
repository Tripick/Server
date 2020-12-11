using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class polygonOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapTile",
                table: "MapTile");

            migrationBuilder.RenameTable(
                name: "MapTile",
                newName: "Tiles");

            migrationBuilder.RenameIndex(
                name: "IX_MapTile_IdTrip",
                table: "Tiles",
                newName: "IX_Tiles_IdTrip");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "MapPoint",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tiles",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "MapPoint");

            migrationBuilder.RenameTable(
                name: "Tiles",
                newName: "MapTile");

            migrationBuilder.RenameIndex(
                name: "IX_Tiles_IdTrip",
                table: "MapTile",
                newName: "IX_MapTile_IdTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapTile",
                table: "MapTile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
