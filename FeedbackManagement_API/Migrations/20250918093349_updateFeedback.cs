using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class updateFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Feedback",
                newName: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Feedback",
                newName: "UserId");
        }
    }
}
