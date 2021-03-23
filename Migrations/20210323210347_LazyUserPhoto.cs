using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class LazyUserPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Location_IdTrip",
                table: "Locations",
                newName: "IX_Locations_IdTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Trips_IdTrip",
                table: "Locations",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Trips_IdTrip",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_IdTrip",
                table: "Location",
                newName: "IX_Location_IdTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Trips_IdTrip",
                table: "Location",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
