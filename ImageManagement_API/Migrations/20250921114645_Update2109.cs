using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class Update2109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTourAgencyImg",
                table: "Images",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVehicleAgencyImage",
                table: "Images",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTourAgencyImg",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsVehicleAgencyImage",
                table: "Images");
        }
    }
}
