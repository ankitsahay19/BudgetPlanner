using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BpstEdu.DBModels;
using BpstEdu.Data;

namespace BpstEdu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BatchesController : Controller
    {
        private readonly AppDbContext _context;

        public BatchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Batches
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Batches.Include(b => b.Course);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.Course)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Admin/Batches/Create
        public async Task<IActionResult> Create(int id)
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "UniqueId", "CourseName");

            Batch? _batch = null;
            if (id > 0)
                _batch = await _context.Batches.FindAsync(id);
            _batch ??= new Batch
            {
                StartingFrom = DateTime.Now.AddDays(15),
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                TenureInDays = 45,
                Fees = 6500,
            };
            return View(_batch);
        }

        // POST: Admin/Batches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                if (batch.UniqueId.Equals(0))
                {
                    batch.CreatedDate = DateTime.Now;
                    _context.Add(batch);
                }
                else _context.Update(batch);
                batch.LastUpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "UniqueId", "CourseName", batch.CourseId);
            return View(batch);
        }


        // GET: Admin/Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.Course)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Admin/Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _context.Batches.Any(e => e.UniqueId == id);
        }
    }
}
