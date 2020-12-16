using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class CleanFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_OwnerId",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_OwnerId",
                table: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Friendship");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Friendship",
                newName: "IdFriend");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "Friendship",
                newName: "IdOwner");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                columns: new[] { "IdOwner", "IdFriend" });

            migrationBuilder.CreateTable(
                name: "AppUserTrip",
                columns: table => new
                {
                    GuestTripsId = table.Column<int>(type: "integer", nullable: false),
                    MembersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTrip", x => new { x.GuestTripsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_AppUserTrip_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserTrip_Trips_GuestTripsId",
                        column: x => x.GuestTripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTrip_MembersId",
                table: "AppUserTrip",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_IdOwner",
                table: "Friendship",
                column: "IdOwner",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_IdOwner",
                table: "Friendship");

            migrationBuilder.DropTable(
                name: "AppUserTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.RenameColumn(
                name: "IdFriend",
                table: "Friendship",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "IdOwner",
                table: "Friendship",
                newName: "FriendId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Friendship",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_OwnerId",
                table: "Friendship",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_OwnerId",
                table: "Friendship",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
