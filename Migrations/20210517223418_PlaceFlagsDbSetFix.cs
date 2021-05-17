using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class PlaceFlagsDbSetFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlagPlace_ConfigReviewFlags_IdConfig",
                table: "FlagPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_FlagPlace_Places_IdPlace",
                table: "FlagPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlagPlace",
                table: "FlagPlace");

            migrationBuilder.RenameTable(
                name: "FlagPlace",
                newName: "PlaceFlags");

            migrationBuilder.RenameIndex(
                name: "IX_FlagPlace_IdPlace",
                table: "PlaceFlags",
                newName: "IX_PlaceFlags_IdPlace");

            migrationBuilder.RenameIndex(
                name: "IX_FlagPlace_IdConfig",
                table: "PlaceFlags",
                newName: "IX_PlaceFlags_IdConfig");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceFlags",
                table: "PlaceFlags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlags_ConfigReviewFlags_IdConfig",
                table: "PlaceFlags",
                column: "IdConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags",
                column: "IdPlace",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlags_ConfigReviewFlags_IdConfig",
                table: "PlaceFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlags_Places_IdPlace",
                table: "PlaceFlags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceFlags",
                table: "PlaceFlags");

            migrationBuilder.RenameTable(
                name: "PlaceFlags",
                newName: "FlagPlace");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceFlags_IdPlace",
                table: "FlagPlace",
                newName: "IX_FlagPlace_IdPlace");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceFlags_IdConfig",
                table: "FlagPlace",
                newName: "IX_FlagPlace_IdConfig");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlagPlace",
                table: "FlagPlace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlagPlace_ConfigReviewFlags_IdConfig",
                table: "FlagPlace",
                column: "IdConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlagPlace_Places_IdPlace",
                table: "FlagPlace",
                column: "IdPlace",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
