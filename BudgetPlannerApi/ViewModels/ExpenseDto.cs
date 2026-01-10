using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    public class ExpenseDto
    {
        public int UniqueId { get; set; }
        public int? UserId { get; set; }
        public int ExpensePlanId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
