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
    public class BatchStudentsController : Controller
    {
        private readonly AppDbContext _context;

        public BatchStudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BatchStudents
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BatchStudents.Include(b => b.Batch).Include(b => b.Student);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/BatchStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchStudent = await _context.BatchStudents
                .Include(b => b.Batch)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (batchStudent == null)
            {
                return NotFound();
            }

            return View(batchStudent);
        }

        // GET: Admin/BatchStudents/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_context.Batches, "UniqueId", "UniqueId");
            ViewData["StudentId"] = new SelectList(_context.Students, "UniqueId", "AadhaarNumber");
            return View();
        }

        // POST: Admin/BatchStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,BatchId,StudentId,DiscountedFeeAmount,CreatedDate,LastUpdatedDate")] BatchStudent batchStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batchStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Batches, "UniqueId", "UniqueId", batchStudent.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Students, "UniqueId", "AadhaarNumber", batchStudent.StudentId);
            return View(batchStudent);
        }

        // GET: Admin/BatchStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchStudent = await _context.BatchStudents.FindAsync(id);
            if (batchStudent == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Batches, "UniqueId", "UniqueId", batchStudent.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Students, "UniqueId", "AadhaarNumber", batchStudent.StudentId);
            return View(batchStudent);
        }

        // POST: Admin/BatchStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UniqueId,BatchId,StudentId,DiscountedFeeAmount,CreatedDate,LastUpdatedDate")] BatchStudent batchStudent)
        {
            if (id != batchStudent.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batchStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchStudentExists(batchStudent.UniqueId))
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
            ViewData["BatchId"] = new SelectList(_context.Batches, "UniqueId", "UniqueId", batchStudent.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Students, "UniqueId", "AadhaarNumber", batchStudent.StudentId);
            return View(batchStudent);
        }

        // GET: Admin/BatchStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchStudent = await _context.BatchStudents
                .Include(b => b.Batch)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (batchStudent == null)
            {
                return NotFound();
            }

            return View(batchStudent);
        }

        // POST: Admin/BatchStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batchStudent = await _context.BatchStudents.FindAsync(id);
            if (batchStudent != null)
            {
                _context.BatchStudents.Remove(batchStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchStudentExists(int id)
        {
            return _context.BatchStudents.Any(e => e.UniqueId == id);
        }
    }
}
