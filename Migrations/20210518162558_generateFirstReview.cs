using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class generateFirstReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_ReviewPlace_IdReview",
                table: "ReviewFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImages_ReviewPlace_IdReview",
                table: "ReviewImages");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteReviewPlace_ReviewPlace_IdReviewPlace",
                table: "VoteReviewPlace");

            migrationBuilder.DropTable(
                name: "ReviewPlace");

            migrationBuilder.CreateTable(
                name: "PlaceReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdPlace = table.Column<int>(type: "integer", nullable: false),
                    IdAuthor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceReviews_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceReviews_Places_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceReviews_IdAuthor",
                table: "PlaceReviews",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceReviews_IdPlace",
                table: "PlaceReviews",
                column: "IdPlace");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewFlags_PlaceReviews_IdReview",
                table: "ReviewFlags",
                column: "IdReview",
                principalTable: "PlaceReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewImages_PlaceReviews_IdReview",
                table: "ReviewImages",
                column: "IdReview",
                principalTable: "PlaceReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteReviewPlace_PlaceReviews_IdReviewPlace",
                table: "VoteReviewPlace",
                column: "IdReviewPlace",
                principalTable: "PlaceReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewFlags_PlaceReviews_IdReview",
                table: "ReviewFlags");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImages_PlaceReviews_IdReview",
                table: "ReviewImages");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteReviewPlace_PlaceReviews_IdReviewPlace",
                table: "VoteReviewPlace");

            migrationBuilder.DropTable(
                name: "PlaceReviews");

            migrationBuilder.CreateTable(
                name: "ReviewPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdAuthor = table.Column<int>(type: "integer", nullable: false),
                    IdPlace = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewPlace_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewPlace_Places_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPlace_IdAuthor",
                table: "ReviewPlace",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPlace_IdPlace",
                table: "ReviewPlace",
                column: "IdPlace");

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

            migrationBuilder.AddForeignKey(
                name: "FK_VoteReviewPlace_ReviewPlace_IdReviewPlace",
                table: "VoteReviewPlace",
                column: "IdReviewPlace",
                principalTable: "ReviewPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
