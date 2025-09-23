using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class update2309v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfBedrooms",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfBedrooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "TotalRooms",
                table: "Rooms");
        }
    }
}
