using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class latlonDelta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EndLatitudeDelta",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EndLongitudeDelta",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StartLatitudeDelta",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StartLongitudeDelta",
                table: "Trips",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndLatitudeDelta",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EndLongitudeDelta",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartLatitudeDelta",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartLongitudeDelta",
                table: "Trips");
        }
    }
}
