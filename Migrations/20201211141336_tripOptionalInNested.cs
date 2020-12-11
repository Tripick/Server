using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class tripOptionalInNested : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapTile",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapPoint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "Location",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile");

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapTile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "MapPoint",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdTrip",
                table: "Location",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapPoint_Trips_IdTrip",
                table: "MapPoint",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapTile_Trips_IdTrip",
                table: "MapTile",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
