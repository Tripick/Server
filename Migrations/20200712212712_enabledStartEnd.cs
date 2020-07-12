using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class enabledStartEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateMode",
                table: "Trips");

            migrationBuilder.AddColumn<bool>(
                name: "EnabledEndDate",
                table: "Trips",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledEndPoint",
                table: "Trips",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledNumberOfDays",
                table: "Trips",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledStartDate",
                table: "Trips",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledStartPoint",
                table: "Trips",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnabledEndDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EnabledEndPoint",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EnabledNumberOfDays",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EnabledStartDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EnabledStartPoint",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "DateMode",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
