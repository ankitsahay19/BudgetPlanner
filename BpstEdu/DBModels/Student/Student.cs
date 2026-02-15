using BpstEdu.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BpstEdu.DBModels.Student
{
    public class Student
    {


        [Key]
        public int UniqueId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application? Application { get; set; }
        public int? ApplicationId { get; set; } 
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        public string FullName { get { return $" {FirstName}  {LastName}"; } }
        [Required(ErrorMessage = "Please select gender")]

        public required string Gender { get; set; } 
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? CollegeName { get; set; } = string.Empty;
 

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;


        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email ID")]
        public string? EmailId { get; set; } = string.Empty;

        public string? HighestQualification { get; set; } = string.Empty;

        public string? Message { get; set; } = string.Empty;

        // Path to uploaded photo (relative to wwwroot)
        public string? PhotoPath { get; set; } = string.Empty;




        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Invalid Aadhaar number. It must be a 12-digit number.")]
        [Display(Name = "Aadhaar Num.")]
        public string? AadhaarNumber { get; set; } = string.Empty;
        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Aadhar")]
        public IFormFile? Aadhar { get; set; } 
        public string? AadharFileUrl { get; set; }



        [Required]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}$", ErrorMessage = "Invalid PA" +
            "N number. Format should be: XXXXX1234X.")]
        [Display(Name = "Pan Num.")]
        public string? PanNumber { get; set; } = string.Empty; 
        [NotMapped]
        [Display(Name = "Upload Pan")]
        public IFormFile? Pan { get; set; } 
        public string? PanFileUrl { get; set; }
    }
}
