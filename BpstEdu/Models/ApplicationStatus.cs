using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models
{
    public  class ApplicationStatus
    { 
        [Key]
        public int UniqueId { get; set; }

        [Required]
        public string RegistrationStatus { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }
    }
}
