using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BpstEdu.Models
{
    public class Application
    {
        [Key]
        public int UniqueId { get; set; }
        public string ApplicationId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
       
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string FullName { get { return $" {FirstName}  {LastName}"; } }
        public string FatherName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int StatusId { get; set; }
        //[ForeignKey("StatusId")]
        //public ApplicationStatus? ApplicationStatus { get; set; }
        public DateTime AppliedOn { get; set; }
        [NotMapped]
        public int NumberOfDays { get { return (AppliedOn - DateTime.Now).Days; } }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        public string HRComment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; } = string.Empty;

        public string CollegeName { get; set; } = string.Empty;

        public string HighestQualification { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }

        //[ForeignKey("CourseId")]
        //public virtual Course? Course { get; set; }
        public string? Message { get; set; } = string.Empty;
        public string? ErrorLogDuringStudentGenration { get; set; } = string.Empty;
    }
}