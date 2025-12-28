using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Bpst.API.Controllers
{
    // Simple MVC controller that serves the SPA index.html (from wwwroot) or a Razor view later
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var indexFile = Path.Combine(wwwRoot, "index.html");

            if (System.IO.File.Exists(indexFile))
            {
                // Serve the static React index.html if present in wwwroot
                return PhysicalFile(indexFile, "text/html");
            }

            // Fallback: simple HTML so the route shows something if no React build was copied
            var html = "<html><head><title>BudgetPlanner</title></head><body><h1>BudgetPlanner</h1><p>Place your React build in wwwroot/ and refresh.</p></body></html>";
            return Content(html, "text/html");
        }
    }
}
