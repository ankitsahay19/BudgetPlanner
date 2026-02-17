using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BpstEdu.DBModels.Student;
using BpstEdu.Data;

namespace BpstEdu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public StudentsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Students
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Students.Include(s => s.Application);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Application)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Admin/Students/Create
        public IActionResult Create()
        {
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName");
            return View();
        }

        // POST: Admin/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,ApplicationId,FirstName,LastName,Gender,DateOfBirth,Address,CollegeName,MobileNumber,EmailId,HighestQualification,Message,PhotoPath,AadhaarNumber,AadharFileUrl,PanNumber,PanFileUrl")] Student student, IFormFile Photo, IFormFile Aadhar, IFormFile Pan)
        {
            if (ModelState.IsValid)
            {
                // if ApplicationId is not provided or is <= 0, set it to null to avoid FK constraint failures
                if (student.ApplicationId.GetValueOrDefault() <= 0)
                {
                    student.ApplicationId = null;
                }
                // First save student to get the UniqueId
                _context.Add(student);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    // capture inner exception details to show in UI for debugging
                    var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                    ModelState.AddModelError(string.Empty, "Database save error: " + inner);
                    ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName", student.ApplicationId);
                    return View(student);
                }

                // Prepare upload folder
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "students", student.UniqueId.ToString());
                if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

                // helper to save file and return relative path
                string SaveFile(IFormFile file, string prefix)
                {
                    if (file == null || file.Length == 0) return null;
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = prefix + "_" + Guid.NewGuid().ToString("N") + ext;
                    var fullPath = Path.Combine(uploadsRoot, fileName);
                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        file.CopyTo(stream);
                    }
                    return Path.Combine("/uploads/students/", student.UniqueId.ToString(), fileName).Replace("\\", "/");
                }

                // Save files and update student paths
                var photoPath = SaveFile(Photo, "photo");
                var aadharPath = SaveFile(Aadhar, "aadhar");
                var panPath = SaveFile(Pan, "pan");

                if (!string.IsNullOrEmpty(photoPath)) student.PhotoPath = photoPath;
                if (!string.IsNullOrEmpty(aadharPath)) student.AadharFileUrl = aadharPath;
                if (!string.IsNullOrEmpty(panPath)) student.PanFileUrl = panPath;

                _context.Update(student);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                    ModelState.AddModelError(string.Empty, "Database update error: " + inner);
                    ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName", student.ApplicationId);
                    return View(student);
                }

                // Redirect to payment wizard (StudentFees Create) with studentId
                return RedirectToAction("Create", "StudentFees", new { area = "Admin", studentId = student.UniqueId });
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName", student.ApplicationId);
            return View(student);
        }

        // GET: Admin/Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName", student.ApplicationId);
            return View(student);
        }

        // POST: Admin/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UniqueId,ApplicationId,FirstName,LastName,Gender,DateOfBirth,Address,CollegeName,MobileNumber,EmailId,HighestQualification,Message,PhotoPath,AadhaarNumber,AadharFileUrl,PanNumber,PanFileUrl")] Student student)
        {
            if (id != student.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "UniqueId", "FirstName", student.ApplicationId);
            return View(student);
        }

        // GET: Admin/Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Application)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Admin/Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.UniqueId == id);
        }
    }
}
