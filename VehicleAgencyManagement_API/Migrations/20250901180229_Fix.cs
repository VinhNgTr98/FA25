using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleAgencyManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TourAgencyId",
                table: "VehicleAgencies",
                newName: "VehicleAgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleAgencyId",
                table: "VehicleAgencies",
                newName: "TourAgencyId");
        }
    }
}
