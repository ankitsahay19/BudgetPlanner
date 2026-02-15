using BpstEdu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BpstEdu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
     public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
             return View();
        }

        public IActionResult Index()
        {
            var applications = _context.Applications.ToList();
            return View(applications);
        }

        public IActionResult Details(int id)
        {
            var application = _context.Applications.Find(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        [HttpPost]
        public IActionResult AddFeedback(int UniqueId, string Feedback, Data.CommonConstants.ApplicationStatus status)
        {
            var application = _context.Applications.Find(UniqueId);
            if (application == null)
            {
                return NotFound();
            }

            application.Feedback = Feedback;
            application.ApplicationStatus = status;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
