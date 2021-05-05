using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class FlagConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigReviewFlags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdReview = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ValType = table.Column<string>(type: "text", nullable: true),
                    MaxLength = table.Column<int>(type: "integer", nullable: false),
                    MinVal = table.Column<int>(type: "integer", nullable: false),
                    MaxVal = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigReviewFlags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true),
                    IdReview = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewImage_ReviewPlace_IdReview",
                        column: x => x.IdReview,
                        principalTable: "ReviewPlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewFlag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    IdReviewFlagConfig = table.Column<int>(type: "integer", nullable: false),
                    IdReview = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewFlag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewFlag_ConfigReviewFlags_IdReviewFlagConfig",
                        column: x => x.IdReviewFlagConfig,
                        principalTable: "ConfigReviewFlags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewFlag_ReviewPlace_IdReview",
                        column: x => x.IdReview,
                        principalTable: "ReviewPlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewFlag_IdReview",
                table: "ReviewFlag",
                column: "IdReview");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewFlag_IdReviewFlagConfig",
                table: "ReviewFlag",
                column: "IdReviewFlagConfig");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewImage_IdReview",
                table: "ReviewImage",
                column: "IdReview");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewFlag");

            migrationBuilder.DropTable(
                name: "ReviewImage");

            migrationBuilder.DropTable(
                name: "ConfigReviewFlags");
        }
    }
}
