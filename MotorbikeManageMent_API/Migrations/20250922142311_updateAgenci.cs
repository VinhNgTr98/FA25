using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeManageMent_API.Migrations
{
    /// <inheritdoc />
    public partial class updateAgenci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MotorbikeAgencyId",
                table: "Motorbike",
                newName: "VehicleAgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleAgencyId",
                table: "Motorbike",
                newName: "MotorbikeAgencyId");
        }
    }
}
