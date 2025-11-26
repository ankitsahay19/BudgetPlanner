using Bpst.API.ViewModels;
using BudgetPlannerApi.DB.Models.User;

namespace Bpst.API.Services.UserAccount
{
    public interface IUserAccountService
    {
        public Task<bool> IfUserExists(string email);
        public Task<AppUser> GetUserByEmail(string email);
        public Task<UserRegistrationResponse> RegisterNewUserAsync(UserRegistrationVM user);
        public Task<LoginResponse> Login(LoginVM login);
        public Task<LoginResponse?> RefreshToken(string refreshToken);
        public Task<UpdateResponse> UpdateEmail(string oldEmail, string newemail, string password);
        public Task<UpdateResponse> UpdatePassword(string email, string oldPassword, string newPassword, string confirmPassword);
        public Task<UserBudgetAllData> GetUserAllData();
        public Task<MonthlyBudget> GetUserMonthlyData(int year, int month);

        public int? GetLoggedInUserId();

    }
}
