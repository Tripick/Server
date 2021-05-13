using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class CountriesAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Places",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CountryAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MinLat = table.Column<double>(type: "double precision", nullable: false),
                    MinLon = table.Column<double>(type: "double precision", nullable: false),
                    MaxLat = table.Column<double>(type: "double precision", nullable: false),
                    MaxLon = table.Column<double>(type: "double precision", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryAreas_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryAreas_CountryId",
                table: "CountryAreas",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryAreas");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Places");
        }
    }
}
