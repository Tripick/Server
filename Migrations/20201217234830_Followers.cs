using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class Followers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserTrip1",
                columns: table => new
                {
                    SubscribersId = table.Column<int>(type: "integer", nullable: false),
                    WatchedTripsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTrip1", x => new { x.SubscribersId, x.WatchedTripsId });
                    table.ForeignKey(
                        name: "FK_AppUserTrip1_AspNetUsers_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserTrip1_Trips_WatchedTripsId",
                        column: x => x.WatchedTripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTrip1_WatchedTripsId",
                table: "AppUserTrip1",
                column: "WatchedTripsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserTrip1");
        }
    }
}
