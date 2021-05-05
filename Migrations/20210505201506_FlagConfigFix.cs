using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class FlagConfigFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdReview",
                table: "ConfigReviewFlags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdReview",
                table: "ConfigReviewFlags",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
