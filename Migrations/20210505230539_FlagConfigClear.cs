using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class FlagConfigClear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlag_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlag");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlag_ReviewPlace_IdReview",
                table: "ReviewFlag");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImage_ReviewPlace_IdReview",
                table: "ReviewImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewImage",
                table: "ReviewImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewFlag",
                table: "ReviewFlag");

            migrationBuilder.RenameTable(
                name: "ReviewImage",
                newName: "ReviewImages");

            migrationBuilder.RenameTable(
                name: "ReviewFlag",
                newName: "ReviewFlags");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewImage_IdReview",
                table: "ReviewImages",
                newName: "IX_ReviewImages_IdReview");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlag_IdReviewFlagConfig",
                table: "ReviewFlags",
                newName: "IX_ReviewFlags_IdReviewFlagConfig");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlag_IdReview",
                table: "ReviewFlags",
                newName: "IX_ReviewFlags_IdReview");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewImages",
                table: "ReviewImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewFlags",
                table: "ReviewFlags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlags",
                column: "IdReviewFlagConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlags_ReviewPlace_IdReview",
                table: "ReviewFlags",
                column: "IdReview",
                principalTable: "ReviewPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewImages_ReviewPlace_IdReview",
                table: "ReviewImages",
                column: "IdReview",
                principalTable: "ReviewPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_ReviewPlace_IdReview",
                table: "ReviewFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImages_ReviewPlace_IdReview",
                table: "ReviewImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewImages",
                table: "ReviewImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewFlags",
                table: "ReviewFlags");

            migrationBuilder.RenameTable(
                name: "ReviewImages",
                newName: "ReviewImage");

            migrationBuilder.RenameTable(
                name: "ReviewFlags",
                newName: "ReviewFlag");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewImages_IdReview",
                table: "ReviewImage",
                newName: "IX_ReviewImage_IdReview");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlag",
                newName: "IX_ReviewFlag_IdReviewFlagConfig");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewFlags_IdReview",
                table: "ReviewFlag",
                newName: "IX_ReviewFlag_IdReview");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewImage",
                table: "ReviewImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewFlag",
                table: "ReviewFlag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlag_ConfigReviewFlags_IdReviewFlagConfig",
                table: "ReviewFlag",
                column: "IdReviewFlagConfig",
                principalTable: "ConfigReviewFlags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlag_ReviewPlace_IdReview",
                table: "ReviewFlag",
                column: "IdReview",
                principalTable: "ReviewPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewImage_ReviewPlace_IdReview",
                table: "ReviewImage",
                column: "IdReview",
                principalTable: "ReviewPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
