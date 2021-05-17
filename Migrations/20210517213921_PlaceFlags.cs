using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class PlaceFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlags");

            migrationBuilder.RenameColumn(
                name: "IdReviewFlagConfig",
                table: "ReviewFlags",
                newName: "IdConfig");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlags",
                newName: "IX_ReviewFlags_IdConfig");

            migrationBuilder.CreateTable(
                name: "PlaceFlag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    IdConfig = table.Column<int>(type: "integer", nullable: false),
                    IdPlace = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceFlag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceFlag_ConfigReviewFlags_IdConfig",
                        column: x => x.IdConfig,
                        principalTable: "ConfigReviewFlags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceFlag_Places_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceFlag_IdConfig",
                table: "PlaceFlag",
                column: "IdConfig");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceFlag_IdPlace",
                table: "PlaceFlag",
                column: "IdPlace");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdConfig",
                table: "ReviewFlags",
                column: "IdConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdConfig",
                table: "ReviewFlags");

            migrationBuilder.DropTable(
                name: "PlaceFlag");

            migrationBuilder.RenameColumn(
                name: "IdConfig",
                table: "ReviewFlags",
                newName: "IdReviewFlagConfig");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlags_IdConfig",
                table: "ReviewFlags",
                newName: "IX_ReviewFlags_IdReviewFlagConfig");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlags",
                column: "IdReviewFlagConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
