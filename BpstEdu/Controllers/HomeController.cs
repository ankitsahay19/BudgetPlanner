using BpstEdu.Data;
using BpstEdu.DBModels;
using BpstEdu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;

namespace BpstEdu.Controllers
{
    public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly AppDbContext _context = context;
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
             return View(new Application()
            {
                ApplicationId = "test" 
            });
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
                     //   application.StatusId = 1;
                     //   var id = _context.Applications.Count();
                     //   application.ApplicationId = "BPST0" + (id + 1); // Increment id to avoid duplicate ID
                        TempData["RegId"] = application.ApplicationId;
                        application.CreatedDate = DateTime.UtcNow.AddMinutes(750);

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
                            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                            var fileName = $"app_{DateTime.UtcNow.Ticks}{Path.GetExtension(Photo.FileName)}";
                            var filePath = Path.Combine(uploads, fileName);
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await Photo.CopyToAsync(stream);
                            }
                            application.PhotoPath = "/uploads/" + fileName;
                        }
                   //     application.AppliedOn = DateTime.UtcNow.AddMinutes(750); // Use UTC for consistency
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
                    //ModelState.AddModelError("", ex.Message);
                    //ModelState.AddModelError("", ex.InnerException.Message);
                    //ModelState.AddModelError("", ex.StackTrace);
                }
            }

            // If we got this far, something failed, redisplay form
          //  ViewData["ApplicationFor"] = new SelectList(_context.Courses, "UniqueId", "Name");
            //ViewData["Qualification"] = new SelectList(_context.Set<Qualification>(), "UniqueId", "Name");
         //   ViewData["courses"] = await _context.Courses.ToListAsync();
            return View(application);
        }
    }

}
