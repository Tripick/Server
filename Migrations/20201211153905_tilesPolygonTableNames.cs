using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class tilesPolygonTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "Tiles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapPoint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "Tiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapPoint",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Trips_IdTrip",
                table: "Tiles",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
