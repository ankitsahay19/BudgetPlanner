using Bpst.API.DB;
using Bpst.API.ViewModels;
using BudgetPlannerApi.DB.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bpst.API.Services.UserAccount
{
    public class UserAccountService(AppDbContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor) : IUserAccountService
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<bool> IfUserExists(string email)
        {
            var user = await _context.AppUsers.AnyAsync(u => u.LoginEmail.Equals(email));
            return user;
        }
        public async Task<AppUser> GetUserByEmail(string email)
        {
            try
            {
                return await _context.AppUsers.Where(u => u.LoginEmail.Equals(email)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return null;
        }



        public async Task<UserRegistrationResponse> RegisterNewUserAsync(UserRegistrationVM user)
        {
            var response = new UserRegistrationResponse() { };

            if (await IfUserExists(user.Email))
            {
                response.ErrorMessages = new List<string>() { "User already exist with given Id, please try with different email" };
                return response;
            }
            else
            {
                var appUser = new AppUser()
                {
                    CreatedDate = DateTime.Now,
                    LoginEmail = user.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    FirstName = user.FirstName,
                    LastName = user.LastLame,
                    PhoneNumber = user.PhoneNumber,
                    Roles = user.Roles,
                };
                _context.AppUsers.Add(appUser);
                try
                {
                    await _context.SaveChangesAsync();
                    string msg = "Dear User,\r<br>\r<br>Welcome... ";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    if (response.ErrorMessages == null)
                        response.ErrorMessages = new List<string>();
                    response.ErrorMessages.Add(ex.Message);
                    response.ErrorMessages.Add(ex.InnerException.Message);
                    response.ErrorMessages.Add(ex.StackTrace);
                    return response;
                }
                response.UniqueId = appUser.UniqueId;
                response.IsCreated = true;
                return response;
            }
        }
        public async Task<LoginResponse> Login(LoginVM login)
        {
            var user = await GetUserByEmail(login.LoginName);
            if (user == null)
                return new LoginResponse { IsLoginSuccess = false, ErrorMessages = new List<string> { "User not registered" } };

            bool valid = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
            if (!valid)
                return new LoginResponse { IsLoginSuccess = false, ErrorMessages = new List<string> { "Invalid credentials" } };

            return await PopulateLoginResponse(user);
        }

        public int? GetLoggedInUserId()
        {
            var User = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        private string CreateJwtToken(AppUser _appUser)
        {
            var userClaims = _appUser.Roles?.Select(r => new Claim(ClaimTypes.Role, r)).ToList() ?? [];

            userClaims.Add(new Claim(ClaimTypes.Name, _appUser.FullName));
            userClaims.Add(new Claim(ClaimTypes.Email, _appUser.LoginEmail));
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, _appUser.UniqueId.ToString()));

            var token = new JwtSecurityToken(
              claims: userClaims,
              expires: DateTime.Now.AddDays(2),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])), SecurityAlgorithms.HmacSha256Signature),
              issuer: _config["JwtSettings:Issuer"],
              audience: _config["JwtSettings:Audience"]
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private LoginResponse ValidateCredentials(AppUser _appUser, string password)
        {
            var response = new LoginResponse() { };
            response.IsLoginSuccess = _appUser == null ? false
                : BCrypt.Net.BCrypt.Verify(password, _appUser.PasswordHash);
            return response;
        }
        private async Task<LoginResponse> PopulateLoginResponse(AppUser user, bool isRefresh = false)
        {
            string refreshToken = isRefresh ? user.RefreshToken! : GenerateRefreshToken();
            if (!isRefresh)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                _context.AppUsers.Update(user);
                await _context.SaveChangesAsync();
            }

            return new LoginResponse
            {
                IsLoginSuccess = true,
                Token = CreateJwtToken(user),
                RefreshToken = refreshToken,
                UserId = user.UniqueId.ToString(),
                FName = user.FirstName,
                LName = user.LastName,
                Email = user.LoginEmail,
                Mobile = user.PhoneNumber,
                IssuedAt = DateTime.UtcNow,
                userRoles = await GetUserRole(user.LoginEmail)
            };
        }


        public async Task<LoginResponse?> RefreshToken(string refreshToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();

            return await PopulateLoginResponse(user, true);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }



        private async Task<List<Roles>?> GetUserRole(string loginEmail)
        {
            var roles = await (from user in _context.AppUsers
                               join userRole in _context.Roles on user.UniqueId equals userRole.UniqueId
                               join role in _context.Roles on userRole.UniqueId equals role.UniqueId
                               where user.LoginEmail == loginEmail
                               select role).ToListAsync();

            return roles;
        }

        public async Task<UpdateResponse> UpdateEmail(string oldEmail, string newemail, string password)
        {
            var _appUser = await GetUserByEmail(newemail);
            if (_appUser == null)
            {
                _appUser = await GetUserByEmail(oldEmail);
                var validationResult = ValidateCredentials(_appUser, password);
                if (validationResult.IsLoginSuccess)
                {
                    _appUser.LoginEmail = newemail;
                    await _context.SaveChangesAsync();
                }
            }
            var result = new UpdateResponse() { };
            var userExist = await IfUserExists(newemail);
            if (userExist)
            {
                result.IsUpdated = false;
                result.ErrorMessages = new List<string>() { "Given login Id: " + newemail + " is already taken by some other user, please try with some different login id" };
            }
            else
            {
                var dbUser = await _context.AppUsers.FindAsync();
                if (dbUser != null)
                {
                    dbUser.LoginEmail = newemail;
                    _context.Entry(dbUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    result.IsUpdated = true;
                }
            }
            return result;
        }

        public async Task<UpdateResponse> UpdatePassword(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var result = new UpdateResponse() { ErrorMessages = [] };

            var _appUser = await GetUserByEmail(email);

            if (_appUser != null)
            {
                var validationResult = ValidateCredentials(_appUser, oldPassword);
                if (validationResult == null || !validationResult.IsLoginSuccess)
                {
                    result.IsUpdated = false;
                    result.ErrorMessages.Add("Old password is incorrect.");
                }
                if (newPassword != confirmPassword)
                {
                    result.IsUpdated = false;
                    result.ErrorMessages.Add("New password and confirm password do not match.");
                }
                else
                {
                    _appUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    _context.AppUsers.Update(_appUser);
                    await _context.SaveChangesAsync();
                }
            }
            result.IsUpdated = true;
            return result;
        }

public async Task<UserBudgetAllData> GetUserAllData()
        {
            var userId = GetLoggedInUserId();
            var currentYear = DateTime.UtcNow.Year;

            var userBudgetData = new UserBudgetAllData
            {
                Year = currentYear,
                Months = new List<Month>()
            };

            for (int month = 1; month <= 12; month++)
            {
                var monthData = new Month
                {
                    MonthValue = month,
                    MonthName = new DateTime(currentYear, month, 1).ToString("MMMM"),
                    Budget = new Budget()
                };

                monthData.Budget.UserDetails = await _context.AppUsers.FindAsync(userId);

                monthData.Budget.IncomeSources = await _context.IncomeSource
                    .Where(i => i.UserId == userId)
                    .ToListAsync();

                monthData.Budget.ExpensePlan = await _context.Categories
                    .Where(c => c.ParentId != null) // Assuming expense categories are sub-categories
                    .ToListAsync();

                monthData.Budget.Expenses = await _context.Expenses
                    .Where(e => e.UserId == userId && e.ExpenseDate.Value.Year == currentYear && e.ExpenseDate.Value.Month == month)
                    .ToListAsync();

                userBudgetData.Months.Add(monthData);
            }

            return userBudgetData;
        }
        
    }
}
