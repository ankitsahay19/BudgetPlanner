using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models
{
    public class UpdatePassword
    {
        [Required]
        [EmailAddress]
        public string? NewEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}
