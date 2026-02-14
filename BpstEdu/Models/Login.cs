using System.ComponentModel.DataAnnotations;

namespace BpstEdu.Models.Users
{
    public class Login
    {
        [Required]
        [Display(Name = "Login UserName")]
        public string LoginUserName { get; set; }

        [Required]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

}
