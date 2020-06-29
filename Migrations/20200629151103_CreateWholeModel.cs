﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TripickServer.Migrations
{
    public partial class CreateWholeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaceId = table.Column<string>(nullable: true),
                    BusinessStatus = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameTranslated = table.Column<string>(nullable: true),
                    PriceLevel = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    NbRating = table.Column<double>(nullable: false),
                    Types = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsPublic = table.Column<bool>(nullable: false),
                    CoverImage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartLatitude = table.Column<double>(nullable: false),
                    StartLongitude = table.Column<double>(nullable: false),
                    EndLatitude = table.Column<double>(nullable: false),
                    EndLongitude = table.Column<double>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleteDate = table.Column<DateTime>(nullable: true),
                    IdOwner = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_AspNetUsers_IdOwner",
                        column: x => x.IdOwner,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeGuide",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeGuide", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeToBrings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeToBrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IdPlace = table.Column<int>(nullable: false),
                    IdUploader = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePlaces_Places_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagePlaces_AspNetUsers_IdUploader",
                        column: x => x.IdUploader,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewPlace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IdPlace = table.Column<int>(nullable: false),
                    IdAuthor = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsMapFrame = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Cover = table.Column<string>(nullable: true),
                    IdTrip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsCustomized = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IdTrip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itineraries_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Picks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<int>(nullable: false),
                    IdPlace = table.Column<int>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdTrip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Picks_Places_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    IdAuthor = table.Column<int>(nullable: false),
                    IdTrip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guides_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Guides_Trips_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Guides_TypeGuide_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeGuide",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoteReviewPlace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsUp = table.Column<bool>(nullable: false),
                    IdReviewPlace = table.Column<int>(nullable: false),
                    IdAuthor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteReviewPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteReviewPlace_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoteReviewPlace_ReviewPlace_IdReviewPlace",
                        column: x => x.IdReviewPlace,
                        principalTable: "ReviewPlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoundingBox",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MinLat = table.Column<double>(nullable: false),
                    MinLon = table.Column<double>(nullable: false),
                    MaxLat = table.Column<double>(nullable: false),
                    MaxLon = table.Column<double>(nullable: false),
                    DestinationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoundingBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoundingBox_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IdItinerary = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Days_Itineraries_IdItinerary",
                        column: x => x.IdItinerary,
                        principalTable: "Itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    GuideId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Hashtags_Guides_GuideId",
                        column: x => x.GuideId,
                        principalTable: "Guides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImageGuides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IdGuide = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageGuides_Guides_IdGuide",
                        column: x => x.IdGuide,
                        principalTable: "Guides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewGuides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IdGuide = table.Column<int>(nullable: false),
                    IdAuthor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewGuides_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewGuides_Guides_IdGuide",
                        column: x => x.IdGuide,
                        principalTable: "Guides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    NbPersons = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    GuideId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeGroups_Guides_GuideId",
                        column: x => x.GuideId,
                        principalTable: "Guides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    DistanceFromPrevious = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCustom = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    IdDay = table.Column<int>(nullable: false),
                    IdPick = table.Column<int>(nullable: true),
                    IdType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Days_IdDay",
                        column: x => x.IdDay,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Steps_Picks_IdPick",
                        column: x => x.IdPick,
                        principalTable: "Picks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Steps_TypeSteps_IdType",
                        column: x => x.IdType,
                        principalTable: "TypeSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToBrings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: false),
                    IdType = table.Column<int>(nullable: true),
                    DayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToBrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToBrings_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToBrings_TypeToBrings_IdType",
                        column: x => x.IdType,
                        principalTable: "TypeToBrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoteReviewGuides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsUp = table.Column<bool>(nullable: false),
                    IdReviewGuide = table.Column<int>(nullable: false),
                    IdAuthor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteReviewGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteReviewGuides_AspNetUsers_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoteReviewGuides_ReviewGuides_IdReviewGuide",
                        column: x => x.IdReviewGuide,
                        principalTable: "ReviewGuides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AppUserId",
                table: "AspNetUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_BoundingBox_DestinationId",
                table: "BoundingBox",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_IdItinerary",
                table: "Days",
                column: "IdItinerary");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_IdTrip",
                table: "Destinations",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_Guides_IdAuthor",
                table: "Guides",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Guides_IdTrip",
                table: "Guides",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_Guides_TypeId",
                table: "Guides",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hashtags_GuideId",
                table: "Hashtags",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGuides_IdGuide",
                table: "ImageGuides",
                column: "IdGuide");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePlaces_IdPlace",
                table: "ImagePlaces",
                column: "IdPlace");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePlaces_IdUploader",
                table: "ImagePlaces",
                column: "IdUploader");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_IdTrip",
                table: "Itineraries",
                column: "IdTrip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Picks_IdPlace",
                table: "Picks",
                column: "IdPlace");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_IdTrip",
                table: "Picks",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_IdUser",
                table: "Picks",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewGuides_IdAuthor",
                table: "ReviewGuides",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewGuides_IdGuide",
                table: "ReviewGuides",
                column: "IdGuide");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPlace_IdAuthor",
                table: "ReviewPlace",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPlace_IdPlace",
                table: "ReviewPlace",
                column: "IdPlace");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_IdDay",
                table: "Steps",
                column: "IdDay");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_IdPick",
                table: "Steps",
                column: "IdPick");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_IdType",
                table: "Steps",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_ToBrings_DayId",
                table: "ToBrings",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_ToBrings_IdType",
                table: "ToBrings",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_IdOwner",
                table: "Trips",
                column: "IdOwner");

            migrationBuilder.CreateIndex(
                name: "IX_TypeGroups_GuideId",
                table: "TypeGroups",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteReviewGuides_IdAuthor",
                table: "VoteReviewGuides",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_VoteReviewGuides_IdReviewGuide",
                table: "VoteReviewGuides",
                column: "IdReviewGuide");

            migrationBuilder.CreateIndex(
                name: "IX_VoteReviewPlace_IdAuthor",
                table: "VoteReviewPlace",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_VoteReviewPlace_IdReviewPlace",
                table: "VoteReviewPlace",
                column: "IdReviewPlace");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AppUserId",
                table: "AspNetUsers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BoundingBox");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "ImageGuides");

            migrationBuilder.DropTable(
                name: "ImagePlaces");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "ToBrings");

            migrationBuilder.DropTable(
                name: "TypeGroups");

            migrationBuilder.DropTable(
                name: "VoteReviewGuides");

            migrationBuilder.DropTable(
                name: "VoteReviewPlace");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Picks");

            migrationBuilder.DropTable(
                name: "TypeSteps");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "TypeToBrings");

            migrationBuilder.DropTable(
                name: "ReviewGuides");

            migrationBuilder.DropTable(
                name: "ReviewPlace");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "Guides");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "TypeGuide");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "AspNetUsers");
        }
    }
}
