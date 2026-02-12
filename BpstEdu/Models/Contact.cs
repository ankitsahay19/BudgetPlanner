using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models
{
    public class Contact
    {
        [Key]
        public int UniqueId { get; set; }
        public string yourName { get; set; }
        public string YourEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}
