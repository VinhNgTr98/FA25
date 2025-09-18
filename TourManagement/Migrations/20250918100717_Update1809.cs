using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourManagement.Migrations
{
    /// <inheritdoc />
    public partial class Update1809 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TourCapacity",
                table: "Tours",
                newName: "MaxCapacity");

            migrationBuilder.RenameColumn(
                name: "TourAgencyID",
                table: "Tours",
                newName: "AgencyID");

            migrationBuilder.RenameColumn(
                name: "Resident",
                table: "Tours",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Tours",
                newName: "BasePrice");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Tours",
                newName: "DurationDays");

            migrationBuilder.RenameColumn(
                name: "Itinerary",
                table: "Tours",
                newName: "Policies");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tours",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StartingPoint",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TourName",
                table: "Tours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tours",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "StartingPoint",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TourName",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tours");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tours",
                newName: "Resident");

            migrationBuilder.RenameColumn(
                name: "Policies",
                table: "Tours",
                newName: "Itinerary");

            migrationBuilder.RenameColumn(
                name: "MaxCapacity",
                table: "Tours",
                newName: "TourCapacity");

            migrationBuilder.RenameColumn(
                name: "DurationDays",
                table: "Tours",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "Tours",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "AgencyID",
                table: "Tours",
                newName: "TourAgencyID");
        }
    }
}
