using BpstEdu.DBModels;
using BpstEdu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BpstEdu.Models.Users;
using BpstEdu.Services;
using BpstEdu.Models;

namespace BpstEdu.Controllers.User
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IUserServiceBAL _userService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IUserServiceBAL userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _userService = userService;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return await ReDirectIfLoggedIn();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginUserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return await ReDirectIfLoggedIn();
                else { ModelState.AddModelError("", "Invalid Email Id or Password"); }
            }
            return View(model);
        }
        public async Task<IActionResult> ReDirectIfLoggedIn()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var role = await _userManager.GetRolesAsync(user);
                if (role.Contains("Admin")) return RedirectToAction("Dashboard", "Home", new { Area = "Admin" });
                else if (role.Contains("Staff")) return RedirectToAction("Index", "Home", new { Area = "Staff" });
                else if (role.Contains("Student")) return RedirectToAction("Index", "Home", new { Area = "Student" });
                else return View("Login");
            }
            else
                return View("Login");// RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Staff,Admin,Student")]

        public async Task<IActionResult> ChangePassword()
        {
            ViewBag.Layout = _userService.GetLayout();
            return View();
        }
        [Authorize(Roles = "Staff,Admin,Student")]

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UpdatePassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpldateLoggedInUserPassword(model.NewEmail, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                    return RedirectToAction(ViewBag.Layout);

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.Layout = _userService.GetLayout();
            return View(model);
        }
        [Authorize(Roles = "Staff,Admin,Student")]

        [HttpGet]
        public async Task<IActionResult> ChangeEmail()
        {
            var emailUpdate = new UpdateEmailVM() { };
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                emailUpdate.OldEmail = user.Email;
            }
            ViewBag.Layout = _userService.GetLayout();
            return View(emailUpdate);
        }
        [Authorize(Roles = "Staff,Admin,Student")]

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(UpdateEmailVM updateEmail)
        {
            var result = await _userService.UpldateLoggedInUserEmail(updateEmail);
            ViewBag.Layout = _userService.GetLayout();
            return View(updateEmail);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //------------api ..........................


        //public async Task<List<City>> GetCities() => await _context.Cities.ToListAsync();
        //public async Task<List<City>> GetCitiesByStateId(int id)
        //{
        //    return await _context.Cities.Where(c => c.StateId == id).ToListAsync();
        //}
        public async Task<City> GetCity(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

    }

}
