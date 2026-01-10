using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    public class WishListDto
    {
        public int UniqueId { get; set; }
        public int? UserId { get; set; }
        [Required]
        public string Item { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
