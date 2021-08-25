using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class PlacesIndexesRatingNbRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Places_NbRating_Rating",
                table: "Places",
                columns: new[] { "NbRating", "Rating" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Places_NbRating_Rating",
                table: "Places");
        }
    }
}
