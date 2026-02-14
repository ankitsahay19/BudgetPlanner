using BpstEdu.DBModels;
using BpstEdu.Data;
using BpstEdu.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using BpstEdu.Models.Users;

namespace BitProSoftTech.Controllers.User
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly HttpContent _httpContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<string> GetCurrentUserId()
        {
            AppUser user = await GetCurrentUserAsync();

            return (user.Id);

        }
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {

            ViewBag.ActiveTabId = 4;
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ActiveTabId = 4;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appUser.UserName = appUser.Email;
                    var result = await _userManager.CreateAsync(appUser, appUser.Password);
                    if (result.Succeeded)
                    {
                        var result2 = await _userManager.AddToRoleAsync(appUser, "Admin");
                        await _signInManager.SignInAsync(appUser, isPersistent: false).ConfigureAwait(false);
                        return RedirectToAction("Index", "Home", new { Areas = "Admin" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return View(appUser);
        }

        public async Task<string> CreateMasterUser()
        {
            var resultStr = string.Empty;
            try
            {
                AppUser appUser = new AppUser()
                {
                    UserName = "admin@bpst.com",
                    Password = "Admin@20",
                    ConfirmPassword = "Admin@20",
                    PhoneNumber = "9999999999",
                };

                var result = await _userManager.CreateAsync(appUser, appUser.Password);
                if (result.Succeeded)
                {
                    var userRoles = _context.Roles.ToList();
                    foreach (var role in userRoles)
                        await _userManager.AddToRoleAsync(appUser, role.Name).ConfigureAwait(false);
                    resultStr = "Master User Created Successfully.";                      
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        resultStr = "Some Error: " + error.Code;
                    }
                }

            }
            catch (Exception ex)
            {
                resultStr = "Some Error: " + ex.Message;
            }
            return resultStr;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // ankit@bitprosoftec.com
            // Admin@20
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginUserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.LoginUserName);
                    var role = await _userManager.GetRolesAsync(user);

                    if (role.Contains("Admin"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Admin" });

                    else if (role.Contains("Hr"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Hr" });
                    else if (role.Contains("Dev"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Dev" });

                    else if (role.Contains("jobapplicant"))
                        return RedirectToAction("Dashboard", "JobApplicant", new { Area = "jobapplicant", });

                    else if (role.Contains("Employee"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Employee" });

                    else if (role.Contains("Accounts"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Accounts" });


                    return RedirectToAction("Dashboard", "Home");
                }
                else { ModelState.AddModelError("", "Invalid Email Id or Password"); }


            }
            ViewBag.ActiveTabId = 4;
            return View(model);
        }

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            ViewBag.ActiveTabId = 1;
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public JsonResult GetCitiesByStateId(int Id)
        //{
        //    var states = _context.Cities.Where(c => c.StateId.Equals(Id)).OrderBy(c => c.Name);
        //    return Json(new SelectList(states, "UniqueId", "Name"));
        //}
    }
}
