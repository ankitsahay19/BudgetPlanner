using Microsoft.AspNetCore.Mvc;
using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using Bpst.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BudgetPlannerApi.DB.Models;

namespace Bpst.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController(AppDbContext context, IUserAccountService userService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IUserAccountService _userService = userService;

        [AllowAnonymous]
        [HttpPost("UserRegistration")]
        public async Task<ActionResult<UserRegistrationResponse>> CreateUser(UserRegistrationVM user)
        {
            var result = await _userService.RegisterNewUserAsync(user);

            if (result.IsCreated == true && result.UniqueId > 0)
            {
                _context.IncomeSource.Add(new IncomeSource() { UserId = result.UniqueId, SourceName = "Salary", IncomeAmount = 0, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow });
                _context.IncomeSource.Add(new IncomeSource() { UserId = result.UniqueId, SourceName = "Freelance", IncomeAmount = 0, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow });
                _context.IncomeSource.Add(new IncomeSource() { UserId = result.UniqueId, SourceName = "Investments", IncomeAmount = 0, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow });
                _context.IncomeSource.Add(new IncomeSource() { UserId = result.UniqueId, SourceName = "Gifts", IncomeAmount = 0, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow });
                _context.SaveChangesAsync().Wait();
            }
            return result;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginVM login)
        {
            var result = await _userService.Login(login);
            return result;
        }

        [Authorize]
        [HttpPost("ChangeLoginEmail")]
        public async Task<ActionResult<UpdateResponse>> ChangeLoginEmail(string newemail, string password)
        {
            string? oldEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            // ToDo, Validate user from existing token and update his login email to new one.
            UpdateResponse result = await _userService.UpdateEmail(oldEmail, newemail, password);
            return result;
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<UpdateResponse>> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            //ToDo, Validate user from existing token and update his password
            var result = await _userService.UpdatePassword(email, oldPassword, newPassword, confirmPassword);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _userService.RefreshToken(refreshToken);
            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }

        [HttpPost("GetUserAllData")]
        public async Task<ActionResult<UserBudgetAllData>> GetUserAllData()
        {
            var result = await _userService.GetUserAllData();
            return Ok(result);
        }

        [HttpPost("GetUserMonthlyData")]
        public async Task<ActionResult<MonthlyBudget>> GetUserMonthlyData(int year, int month)
        {
            var result = await _userService.GetUserMonthlyData(year, month);
            return Ok(result);
        }
    }
}
