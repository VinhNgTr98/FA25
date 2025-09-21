using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourManagement.Migrations
{
    /// <inheritdoc />
    public partial class Update2109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelPolicies",
                table: "Tours",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Excluded",
                table: "Tours",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Included",
                table: "Tours",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Requiments",
                table: "Tours",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    ItineraryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItineraryOrder = table.Column<int>(type: "int", nullable: true),
                    TourID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItineraryTitles = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItineraryDetails = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.ItineraryId);
                    table.ForeignKey(
                        name: "FK_Itineraries_Tours_TourID",
                        column: x => x.TourID,
                        principalTable: "Tours",
                        principalColumn: "TourID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_TourID",
                table: "Itineraries",
                column: "TourID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropColumn(
                name: "CancelPolicies",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Excluded",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Included",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Requiments",
                table: "Tours");
        }
    }
}
