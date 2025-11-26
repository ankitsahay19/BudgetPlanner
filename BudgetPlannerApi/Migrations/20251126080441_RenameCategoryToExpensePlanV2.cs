using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlannerApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategoryToExpensePlanV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlans_Categories_CategoryId",
                table: "BudgetPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "ExpensePlan");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "ExpensePlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ExpensePlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePlan",
                table: "ExpensePlan",
                column: "UniqueId");

            migrationBuilder.UpdateData(
                table: "ExpensePlan",
                keyColumn: "UniqueId",
                keyValue: 1,
                columns: new[] { "Month", "Year" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "ExpensePlan",
                keyColumn: "UniqueId",
                keyValue: 2,
                columns: new[] { "Month", "Year" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "ExpensePlan",
                keyColumn: "UniqueId",
                keyValue: 3,
                columns: new[] { "Month", "Year" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "ExpensePlan",
                keyColumn: "UniqueId",
                keyValue: 4,
                columns: new[] { "Month", "Year" },
                values: new object[] { 0, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlans_ExpensePlan_CategoryId",
                table: "BudgetPlans",
                column: "CategoryId",
                principalTable: "ExpensePlan",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpensePlan_CategoryId",
                table: "Expenses",
                column: "CategoryId",
                principalTable: "ExpensePlan",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlans_ExpensePlan_CategoryId",
                table: "BudgetPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpensePlan_CategoryId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePlan",
                table: "ExpensePlan");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "ExpensePlan");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ExpensePlan");

            migrationBuilder.RenameTable(
                name: "ExpensePlan",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "UniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlans_Categories_CategoryId",
                table: "BudgetPlans",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "UniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
