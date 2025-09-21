using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class Update1809 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tiles",
                table: "Hotel",
                newName: "TaxNumber");

            migrationBuilder.RenameColumn(
                name: "CancellationPolicy",
                table: "Hotel",
                newName: "HotelName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxNumber",
                table: "Hotel",
                newName: "Tiles");

            migrationBuilder.RenameColumn(
                name: "HotelName",
                table: "Hotel",
                newName: "CancellationPolicy");
        }
    }
}
