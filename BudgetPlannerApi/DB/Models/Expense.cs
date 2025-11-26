
using BudgetPlannerApi.DB.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BudgetPlannerApplication_2025.Models
{
    public class Expense
    {
        [Key]
        public int UniqueId { get; set; }
        // Foreign key property
        public int? UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        [JsonIgnore]
        public AppUser? AppUser { get; set; }
        public int ExpensePlanId { get; set; }

        [ForeignKey("ExpensePlanId")]
        public ExpensePlan? ExpensePlan { get; set; }

        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

    }
}
