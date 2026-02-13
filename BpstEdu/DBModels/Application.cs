using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BpstEdu.DBModels
{
    public class Application
    {
        [Key]
        public int UniqueId { get; set; }
        public string ApplicationId { get; set; } = string.Empty;
        public string? CollegeName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string FullName { get { return $" {FirstName}  {LastName}"; } }

        public string FatherName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        [Display(Name = "Applied On")]
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        [Display(Name = "Days Ego")]
        public int NumberOfDays { get { return (CreatedDate - DateTime.Now).Days; } }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        //---------------------------------------------------------------


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter highest qualification")]
        public string HighestQualification { get; set; } = string.Empty;

        public string? Message { get; set; } = string.Empty;

        // Path to uploaded photo (relative to wwwroot)
        public string? PhotoPath { get; set; } = string.Empty;


    }
}