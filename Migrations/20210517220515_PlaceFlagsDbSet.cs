using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class PlaceFlagsDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlag_ConfigReviewFlags_IdConfig",
                table: "PlaceFlag");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceFlag_Places_IdPlace",
                table: "PlaceFlag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceFlag",
                table: "PlaceFlag");

            migrationBuilder.RenameTable(
                name: "PlaceFlag",
                newName: "FlagPlace");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceFlag_IdPlace",
                table: "FlagPlace",
                newName: "IX_FlagPlace_IdPlace");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceFlag_IdConfig",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "PlaceFlag");

            migrationBuilder.RenameIndex(
                name: "IX_FlagPlace_IdPlace",
                table: "PlaceFlag",
                newName: "IX_PlaceFlag_IdPlace");

            migrationBuilder.RenameIndex(
                name: "IX_FlagPlace_IdConfig",
                table: "PlaceFlag",
                newName: "IX_PlaceFlag_IdConfig");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceFlag",
                table: "PlaceFlag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlag_ConfigReviewFlags_IdConfig",
                table: "PlaceFlag",
                column: "IdConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceFlag_Places_IdPlace",
                table: "PlaceFlag",
                column: "IdPlace",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
