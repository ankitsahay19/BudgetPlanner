using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BpstEdu.Migrations
{
    /// <inheritdoc />
    public partial class ifMissing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fees",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 1,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 2,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 3,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 4,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 5,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 6,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 7,
                column: "Fees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "UniqueId",
                keyValue: 8,
                column: "Fees",
                value: 0m);
        }
    }
}
