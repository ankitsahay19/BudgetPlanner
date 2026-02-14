using BpstEdu.Data;
using BpstEdu.DBModels;
using BpstEdu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace BpstEdu.Services
{
    public class UserServiceBAL : IUserServiceBAL
    {
        private readonly AppDbContext _dbContext;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly UserManager<AppUser> _userManager;

        public UserServiceBAL(AppDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            ITempDataDictionaryFactory tempDataDictionaryFactory,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string GetLoggedInUser()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public string GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public string GetLoggedInUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }
        public List<string> GetLoggedInUserRoles()
        {
            var roles = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(roleClaim => roleClaim.Value).ToList();
            return roles;
        }
        public string GetLayout()
        {
            var _layout = string.Empty;
            var roles = GetLoggedInUserRoles();
            if (roles.Contains("Admin"))
            {
                _layout = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else if (roles.Contains("Staff"))
            {
                _layout = "~/Views/Shared/_StaffLayout.cshtml";
            }
            else if (roles.Contains("Student"))
            { 
                _layout = "~/Views/Shared/_StuLayout.cshtml";
            }
            return _layout;
        }
        public async Task<bool> IsUserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
        public async Task<IdentityResult> AddUser(AppUser user, List<string> role)
        {
            var result = await _userManager.CreateAsync(user, user.Password);
            if (result.Succeeded && role != null && role.Any())
                await _userManager.AddToRolesAsync(user, role).ConfigureAwait(false);
            return result;
        }

        public async Task<IdentityResult> UpldateLoggedInUserEmail(UpdateEmailVM updateEmail)
        {
            /// check in user is valid , by validating password of cussent user . 
            // updateEmail.Password; // error --- Incorrect pwd.
            //             return;


            // -- check if new email is not already taken by any other user. 
            // --- this email is already taken by some one else, pease try with some different email.

            //-- Update email successfully .



            // Fetch the user from the database
            var user = await _userManager.FindByEmailAsync(updateEmail.OldEmail);

            // Check if the user is found
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Update the user's email
            user.Email = updateEmail.NewEmail;
            user.UserName = updateEmail.NewEmail;   // Uncomment this if you want to update the username as well

            // Save the changes to the database
            var emailUpdateResult = await _userManager.UpdateAsync(user);

            // Return the result of the email update
            return emailUpdateResult;
        }
        public async Task<IdentityResult> UpldateLoggedInUserPassword(string newEmail, string oldPassword, string newPassword)
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Verify the old password
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, oldPassword, false);
            if (!passwordCheck.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Incorrect old password." });
            }

            // Update the user's email
            user.Email = newEmail;
            // user.UserName = newEmail;
            var emailUpdateResult = await _userManager.UpdateAsync(user);
            if (!emailUpdateResult.Succeeded)
            {
                return emailUpdateResult;
            }

            // Update the user's password
            var passwordUpdateResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!passwordUpdateResult.Succeeded)
            {
                return passwordUpdateResult;
            }

            // Re-sign in the user to refresh the authentication cookie
            await _signInManager.RefreshSignInAsync(user);

            return IdentityResult.Success;
        }
    }
}
