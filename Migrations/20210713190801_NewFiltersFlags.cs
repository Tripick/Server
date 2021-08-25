using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class NewFiltersFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.AlterColumn<int>(
                name: "IdPlace",
                table: "PlaceFlags",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "IdTrip",
                table: "PlaceFlags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceFlags_IdTrip",
                table: "PlaceFlags",
                column: "IdTrip");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags",
                column: "IdPlace",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlags_Trips_IdTrip",
                table: "PlaceFlags",
                column: "IdTrip",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlags_Trips_IdTrip",
                table: "PlaceFlags");

            migrationBuilder.DropIndex(
                name: "IX_PlaceFlags_IdTrip",
                table: "PlaceFlags");

            migrationBuilder.DropColumn(
                name: "IdTrip",
                table: "PlaceFlags");

            migrationBuilder.AlterColumn<int>(
                name: "IdPlace",
                table: "PlaceFlags",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTrip = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    Max = table.Column<int>(type: "integer", nullable: false),
                    Min = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filters_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Filters_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filters_IdTrip",
                table: "Filters",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_IdUser",
                table: "Filters",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags",
                column: "IdPlace",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
