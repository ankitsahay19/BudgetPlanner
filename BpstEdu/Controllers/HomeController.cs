using BpstEdu.Data;
using BpstEdu.DBModels;
using BpstEdu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public static string ReadFile(string FileName)
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
            catch (Exception ex)
            {
                //Log
                throw ex;
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
        //[HttpGet]
        //public async Task<IActionResult> GetBatchesByCourseId(int courseId)
        //{
        //    var batches = await _context.Batchs.Include(b => b.Course)
        //      //  .Where(b => b.CourseId == courseId)
        //        .Select(b => new
        //        {
        //            Id = b.UniqueId,
        //            Value = $"{b.RemainingDays} Days To Go -  {b.Course.Name}_{b.StartDateTime:dd-MMM-yyyy}_{b.StartDateTime:hh:mm: tt}"
        //        }).ToListAsync();
        //    return Json(batches);
        //}
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
                try
                {
                    if (application.UniqueId.Equals(0))
                    {
                        application.CreatedDate = DateTime.UtcNow.AddMinutes(750);

                        // Generate ApplicationId if not already set
                        if (string.IsNullOrEmpty(application.ApplicationId))
                        {
                            var count = _context.Applications.Count();
                            application.ApplicationId = "BPST" + (count + 1).ToString().PadLeft(5, '0');
                        }

                        // Handle uploaded photo
                        if (Photo != null && Photo.Length > 0)
                        {
                            var allowed = new[] { "image/jpeg", "image/png", "image/jpg" };
                            if (!allowed.Contains(Photo.ContentType))
                            {
                                ModelState.AddModelError("Photo", "Only JPG/PNG images are allowed.");
                                return View(application);
                            }
                            if (Photo.Length > 2 * 1024 * 1024)
                            {
                                ModelState.AddModelError("Photo", "Maximum file size is 2MB.");
                                return View(application);
                            }

                            // Create folder structure: wwwroot/applications/{ApplicationId}/
                            var applicationsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "applications", application.ApplicationId);
                            if (!Directory.Exists(applicationsFolder))
                                Directory.CreateDirectory(applicationsFolder);

                            // Generate unique filename with extension
                            var fileExtension = Path.GetExtension(Photo.FileName);
                            var fileName = $"photo_{DateTime.UtcNow.Ticks}{fileExtension}";
                            var filePath = Path.Combine(applicationsFolder, fileName);

                            // Save the file
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await Photo.CopyToAsync(stream);
                            }

                            // Store relative path in database
                            application.PhotoPath = $"/applications/{application.ApplicationId}/{fileName}";
                        }

                        _context.Add(application);
                    }
                    else
                        _context.Update(application);
                    await _context.SaveChangesAsync();
                    TempData["SaveSuccess"] = true;
                    return RedirectToAction(nameof(StudentApplications));
                }
                catch (Exception ex)
                {
                    //if (application.CourseId == 0)
                    //    ModelState.AddModelError("Course", "Please select course ");
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Some thing wrong with Data, unable to save changes. Call To 82-9910-1616 for Registration.");
                }
            }

            return View(application);
        }
    }
}
