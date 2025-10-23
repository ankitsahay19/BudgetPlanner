using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BudgetPlannerApi.DB.Models;
using BudgetPlannerApi.DB.Models.User;
using BudgetPlannerApplication_2025.Models;

[NotMapped]
[ExcludeFromCodeCoverage]
public class Budget
{
    public AppUser? UserDetails { get; set; }
    public List<IncomeSource>? IncomeSources { get; set; }
    public List<Category>? ExpensePlan { get; set; }
    public List<Expense>? Expenses { get; set; }
}

[NotMapped]
[ExcludeFromCodeCoverage]
public class UserBudgetAllData
{
    public int Year { get; set; }
    public List<Month>? Months { get; set; }
}

[NotMapped]
[ExcludeFromCodeCoverage]
public class Month
{
    public int MonthValue { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public Budget? Budget { get; set; }
}