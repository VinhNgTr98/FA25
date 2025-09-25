using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleUpdateManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class update2509 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenceImg",
                table: "RoleUpdateForms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicenceImg",
                table: "RoleUpdateForms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
