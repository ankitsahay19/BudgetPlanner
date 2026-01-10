using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    public class BudgetPlanDto
    {
        public int UniqueId { get; set; }
        public int? UserId { get; set; }
        [Required]
        public int ExpectedAmount { get; set; }
        public int CategoryId { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
