using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models
{
    public class Contact
    {
        [Key]
        public int UniqueId { get; set; }
        public string yourName { get; set; } = string.Empty;
        public string YourEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}
