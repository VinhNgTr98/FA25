using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourAgencyManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TourAgency",
                table: "TourAgency");

            migrationBuilder.RenameTable(
                name: "TourAgency",
                newName: "TourAgencies");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TourAgencies",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourAgencies",
                table: "TourAgencies",
                column: "TourAgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TourAgencies",
                table: "TourAgencies");

            migrationBuilder.RenameTable(
                name: "TourAgencies",
                newName: "TourAgency");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TourAgency",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourAgency",
                table: "TourAgency",
                column: "TourAgencyId");
        }
    }
}
