using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class BackToGuestTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserTrip",
                columns: table => new
                {
                    GuestTripsId = table.Column<int>(type: "integer", nullable: false),
                    MembersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTrip", x => new { x.GuestTripsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_AppUserTrip_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserTrip_Trips_GuestTripsId",
                        column: x => x.GuestTripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTrip_MembersId",
                table: "AppUserTrip",
                column: "MembersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserTrip");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
