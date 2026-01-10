using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    public class ExpensePlanDto
    {
        public int UniqueId { get; set; }
        public int? UserId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public int AllocatedAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
