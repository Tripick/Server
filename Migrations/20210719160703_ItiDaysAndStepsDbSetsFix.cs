using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class ItiDaysAndStepsDbSetsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDays_Itineraries_ItineraryId",
                table: "ItineraryDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDaySteps_ItineraryDays_ItineraryDayId",
                table: "ItineraryDaySteps");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDaySteps_ItineraryDayId",
                table: "ItineraryDaySteps");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDays_ItineraryId",
                table: "ItineraryDays");

            migrationBuilder.DropColumn(
                name: "ItineraryDayId",
                table: "ItineraryDaySteps");

            migrationBuilder.DropColumn(
                name: "ItineraryId",
                table: "ItineraryDays");

            migrationBuilder.AddColumn<int>(
                name: "IdDay",
                table: "ItineraryDaySteps",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdItinerary",
                table: "ItineraryDays",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDaySteps_IdDay",
                table: "ItineraryDaySteps",
                column: "IdDay");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_IdItinerary",
                table: "ItineraryDays",
                column: "IdItinerary");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDays_Itineraries_IdItinerary",
                table: "ItineraryDays",
                column: "IdItinerary",
                principalTable: "Itineraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDaySteps_ItineraryDays_IdDay",
                table: "ItineraryDaySteps",
                column: "IdDay",
                principalTable: "ItineraryDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDays_Itineraries_IdItinerary",
                table: "ItineraryDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDaySteps_ItineraryDays_IdDay",
                table: "ItineraryDaySteps");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDaySteps_IdDay",
                table: "ItineraryDaySteps");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDays_IdItinerary",
                table: "ItineraryDays");

            migrationBuilder.DropColumn(
                name: "IdDay",
                table: "ItineraryDaySteps");

            migrationBuilder.DropColumn(
                name: "IdItinerary",
                table: "ItineraryDays");

            migrationBuilder.AddColumn<int>(
                name: "ItineraryDayId",
                table: "ItineraryDaySteps",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItineraryId",
                table: "ItineraryDays",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDaySteps_ItineraryDayId",
                table: "ItineraryDaySteps",
                column: "ItineraryDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_ItineraryId",
                table: "ItineraryDays",
                column: "ItineraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDays_Itineraries_ItineraryId",
                table: "ItineraryDays",
                column: "ItineraryId",
                principalTable: "Itineraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDaySteps_ItineraryDays_ItineraryDayId",
                table: "ItineraryDaySteps",
                column: "ItineraryDayId",
                principalTable: "ItineraryDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
