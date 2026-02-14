using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models.Users
{
    public class Login
    {
        [Required]
        [Display(Name = "Login UserName")]
        public string LoginUserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

}
