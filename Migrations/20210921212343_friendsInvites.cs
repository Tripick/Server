using Microsoft.EntityFrameworkCore.Migrations;

namespace TripickServer.Migrations
{
    public partial class friendsInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Friendship");

            migrationBuilder.AddColumn<int>(
                name: "NeedToConfirm",
                table: "Friendship",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedToConfirm",
                table: "Friendship");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Friendship",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
