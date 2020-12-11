using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class addTiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "Polygon",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Trips",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapTile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Width = table.Column<double>(nullable: false),
                    IdTrip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapTile_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapTile_IdTrip",
                table: "MapTile",
                column: "IdTrip");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapTile");

            migrationBuilder.DropColumn(
                name: "Polygon",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Trips");

            migrationBuilder.AddColumn<bool>(
                name: "EnabledEndDate",
                table: "Trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledEndPoint",
                table: "Trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledNumberOfDays",
                table: "Trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledStartDate",
                table: "Trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledStartPoint",
                table: "Trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
