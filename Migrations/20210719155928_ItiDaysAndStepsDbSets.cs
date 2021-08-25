using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class ItiDaysAndStepsDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDay_Itineraries_ItineraryId",
                table: "ItineraryDay");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDayStep_ItineraryDay_ItineraryDayId",
                table: "ItineraryDayStep");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDayStep_Picks_IdVisit",
                table: "ItineraryDayStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryDayStep",
                table: "ItineraryDayStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryDay",
                table: "ItineraryDay");

            migrationBuilder.RenameTable(
                name: "ItineraryDayStep",
                newName: "ItineraryDaySteps");

            migrationBuilder.RenameTable(
                name: "ItineraryDay",
                newName: "ItineraryDays");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDayStep_ItineraryDayId",
                table: "ItineraryDaySteps",
                newName: "IX_ItineraryDaySteps_ItineraryDayId");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDayStep_IdVisit",
                table: "ItineraryDaySteps",
                newName: "IX_ItineraryDaySteps_IdVisit");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDay_ItineraryId",
                table: "ItineraryDays",
                newName: "IX_ItineraryDays_ItineraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryDaySteps",
                table: "ItineraryDaySteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryDays",
                table: "ItineraryDays",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDaySteps_Picks_IdVisit",
                table: "ItineraryDaySteps",
                column: "IdVisit",
                principalTable: "Picks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDays_Itineraries_ItineraryId",
                table: "ItineraryDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDaySteps_ItineraryDays_ItineraryDayId",
                table: "ItineraryDaySteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDaySteps_Picks_IdVisit",
                table: "ItineraryDaySteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryDaySteps",
                table: "ItineraryDaySteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryDays",
                table: "ItineraryDays");

            migrationBuilder.RenameTable(
                name: "ItineraryDaySteps",
                newName: "ItineraryDayStep");

            migrationBuilder.RenameTable(
                name: "ItineraryDays",
                newName: "ItineraryDay");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDaySteps_ItineraryDayId",
                table: "ItineraryDayStep",
                newName: "IX_ItineraryDayStep_ItineraryDayId");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDaySteps_IdVisit",
                table: "ItineraryDayStep",
                newName: "IX_ItineraryDayStep_IdVisit");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDays_ItineraryId",
                table: "ItineraryDay",
                newName: "IX_ItineraryDay_ItineraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryDayStep",
                table: "ItineraryDayStep",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryDay",
                table: "ItineraryDay",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDay_Itineraries_ItineraryId",
                table: "ItineraryDay",
                column: "ItineraryId",
                principalTable: "Itineraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDayStep_ItineraryDay_ItineraryDayId",
                table: "ItineraryDayStep",
                column: "ItineraryDayId",
                principalTable: "ItineraryDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDayStep_Picks_IdVisit",
                table: "ItineraryDayStep",
                column: "IdVisit",
                principalTable: "Picks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
