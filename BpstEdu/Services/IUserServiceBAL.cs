using BpstEdu.DBModels.User;
using BpstEdu.Models;
using Microsoft.AspNetCore.Identity;

namespace BpstEdu.Services
{
    public interface IUserServiceBAL
    {
        public string GetLoggedInUser();
        public string GetLoggedInUserId();
        public string GetLoggedInUserEmail();
        public List<string> GetLoggedInUserRoles();
        public string GetLayout();

        public Task<bool> IsUserExist(string email);
        public Task<IdentityResult> AddUser(AppUser user, List<string> role); 
        public Task<IdentityResult> UpldateLoggedInUserPassword(string Email, string oldPassword, string newPassword);
        public Task<IdentityResult> UpldateLoggedInUserEmail(UpdateEmailVM updateEmail);
    }
}
