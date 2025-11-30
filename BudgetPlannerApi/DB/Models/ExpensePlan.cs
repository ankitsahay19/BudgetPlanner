
using BudgetPlannerApi.DB.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BudgetPlannerApplication_2025.Models
{
     [Table("ExpensePlan")]
    public class ExpensePlan
    {
        [Key]
        public int UniqueId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

         public string? Description { get; set; } = string.Empty;

        public int? ParentId { get; set; } // Should be nullable to allow top-level categories  

        public int? UserId { get; set; }
        [NotMapped]
        public ICollection<ExpensePlan>? SubExpensePlans { get; set; }
        public int AllocatedAmount { get; set; }
        [NotMapped]
        public int TotalAllocatedAmountOfSubExpensePlans { get { return SubExpensePlans?.Sum(c => c.AllocatedAmount) ?? 0; } }

        [NotMapped]
        public int RemainingBalance { get { return AllocatedAmount - TotalAllocatedAmountOfSubExpensePlans; } }
         
         [NotMapped]
        public int TotalOfSubCategoriesAmount
        {
            get
            {
                int _allocatedAmount = 0;
                if (SubExpenses != null && SubExpenses.Count > 0)
                    return SubExpenses.Sum(sc => sc.AllocatedAmount);
                return _allocatedAmount;
            }
        } 
        [JsonIgnore]
        // Self-referencing navigation property
        [ForeignKey("ParentId")]
        public ExpensePlan? ParentCategory { get; set; }
        public ICollection<ExpensePlan>? SubExpenses { get; set; }


        public int Month { get; set; }
        public int Year { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

    }

}
