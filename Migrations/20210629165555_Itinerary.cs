using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class Itinerary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItineraryDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    DistanceToStart = table.Column<double>(type: "double precision", nullable: false),
                    DistanceToEnd = table.Column<double>(type: "double precision", nullable: false),
                    ItineraryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryDay_Itineraries_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryDayStep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    IsPassage = table.Column<bool>(type: "boolean", nullable: false),
                    IsStart = table.Column<bool>(type: "boolean", nullable: false),
                    IsEnd = table.Column<bool>(type: "boolean", nullable: false),
                    IsVisit = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuggestion = table.Column<bool>(type: "boolean", nullable: false),
                    DistanceToPassage = table.Column<double>(type: "double precision", nullable: false),
                    VisitLikely = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    VisitId = table.Column<int>(type: "integer", nullable: true),
                    ItineraryDayId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryDayStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryDayStep_ItineraryDay_ItineraryDayId",
                        column: x => x.ItineraryDayId,
                        principalTable: "ItineraryDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItineraryDayStep_Picks_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Picks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDay_ItineraryId",
                table: "ItineraryDay",
                column: "ItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDayStep_ItineraryDayId",
                table: "ItineraryDayStep",
                column: "ItineraryDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDayStep_VisitId",
                table: "ItineraryDayStep",
                column: "VisitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryDayStep");

            migrationBuilder.DropTable(
                name: "ItineraryDay");
        }
    }
}
