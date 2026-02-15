using BpstEdu.Services;
using BpstEdu.Data;
using BpstEdu.DBModels;
using BpstEdu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BpstEdu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IStudentApplicationService _studentApplicationService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IStudentApplicationService studentApplicationService)
        {
            _logger = logger;
            _context = context;
            _studentApplicationService = studentApplicationService;
        }
        public IActionResult Index()
        {
            ViewBag.ActiveTabId = 1;
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View(new Contact());
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {

            if (ModelState.IsValid)
            {
                if (contact.UniqueId.Equals(0))
                {
                    await _context.Contacts.AddRangeAsync(contact);
                    _context.Contacts.Add(contact);

                }
                else

                    _context.Contacts.Update(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(contact));
            }
            return View(contact);
        }
        public IActionResult Carrier()
        {
            ViewBag.ActiveTabId = 3;
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public static string? ReadFile(string FileName)
        {
            try
            {
                using (StreamReader reader = System.IO.File.OpenText(FileName))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "")
                    {
                        return fileContent;
                    }
                }
            }
            catch 
            {
                //Log
                throw;
            }
            return null;
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Exam()
        {
            return View();
        } 
        
        public async Task<IActionResult> StudentApplications()
        {
             return View( );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentApplications(Application application, Microsoft.AspNetCore.Http.IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                var (success, errorMessage) = await _studentApplicationService.ProcessStudentApplicationAsync(application, Photo, ModelState);

                if (success)
                {
                    TempData["SaveSuccess"] = true;
                    return RedirectToAction(nameof(StudentApplications));
                }
                else
                    {
                    ModelState.AddModelError("", errorMessage ?? "An unknown error occurred during application processing.");
                }
            }
            return View(application);
        }
    }
}
