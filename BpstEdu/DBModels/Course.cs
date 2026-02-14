using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models
{
    public class Course
    {
        [Key]
        public int UniqueId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public bool IntershipAvailable { get; set; }
    }
}
