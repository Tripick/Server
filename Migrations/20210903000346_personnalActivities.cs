using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class personnalActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ItineraryDaySteps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ItineraryDaySteps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ItineraryDaySteps",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ItineraryDaySteps");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "ItineraryDaySteps");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ItineraryDaySteps");
        }
    }
}
