using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlannerApi.Migrations
{
    /// <inheritdoc />
    public partial class updatedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAllocatedAmountOfSubCategories",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAllocatedAmountOfSubCategories",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "UniqueId",
                keyValue: 1,
                column: "TotalAllocatedAmountOfSubCategories",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "UniqueId",
                keyValue: 2,
                column: "TotalAllocatedAmountOfSubCategories",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "UniqueId",
                keyValue: 3,
                column: "TotalAllocatedAmountOfSubCategories",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "UniqueId",
                keyValue: 4,
                column: "TotalAllocatedAmountOfSubCategories",
                value: 0);
        }
    }
}
