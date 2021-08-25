using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class ItiVisitFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDayStep_Picks_VisitId",
                table: "ItineraryDayStep");

            migrationBuilder.RenameColumn(
                name: "VisitId",
                table: "ItineraryDayStep",
                newName: "IdVisit");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDayStep_VisitId",
                table: "ItineraryDayStep",
                newName: "IX_ItineraryDayStep_IdVisit");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDayStep_Picks_IdVisit",
                table: "ItineraryDayStep",
                column: "IdVisit",
                principalTable: "Picks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDayStep_Picks_IdVisit",
                table: "ItineraryDayStep");

            migrationBuilder.RenameColumn(
                name: "IdVisit",
                table: "ItineraryDayStep",
                newName: "VisitId");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryDayStep_IdVisit",
                table: "ItineraryDayStep",
                newName: "IX_ItineraryDayStep_VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDayStep_Picks_VisitId",
                table: "ItineraryDayStep",
                column: "VisitId",
                principalTable: "Picks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
