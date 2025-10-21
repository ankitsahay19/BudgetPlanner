
using BudgetPlannerApi.DB.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BudgetPlannerApplication_2025.Models
{
    public class Category
    {
        [Key]
        public int UniqueId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; } // Should be nullable to allow top-level categories

        public string ?Description { get; set; } = string.Empty; 
        public DateTime ?CreatedDate { get; set; }

        public DateTime ?LastUpdatedDate { get; set; }

        // Foreign key property
        public int ?UserId { get; set; }

        // Navigation property to User
        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }

        [JsonIgnore]
        // Self-referencing navigation property
        [ForeignKey("ParentId")]
        public Category? ParentCategory { get; set; }
        public ICollection<Category>? SubCategories { get; set; }
        public int AllocatedAmount { get; set; }
        [NotMapped]
        public int TotalAllocatedAmountOfSubCategories { get { return SubCategories?.Sum(c => c.AllocatedAmount) ?? 0; } }

        [NotMapped]
        public int RemainingBalance { get { return AllocatedAmount - TotalAllocatedAmountOfSubCategories; } } 
         
    }

}
