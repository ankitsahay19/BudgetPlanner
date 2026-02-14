using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BpstEdu.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationStatus",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Applications");
        }
    }
}
