using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    // Single shared DTO for create and update operations for IncomeSource
    public class IncomeSourceDto
    {
        // Present in responses; ignored on create/update by service mapping
        public int UniqueId { get; set; }

        // Owner (returned in responses). Service ignores client-sent UserId on DTO->Entity mapping.
        public int? UserId { get; set; }
        [Required]
        public string SourceName { get; set; } = string.Empty;

        public decimal? IncomeAmount { get; set; }

        // Returned by API; server sets these. Nullable so client can omit when creating/updating.
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
