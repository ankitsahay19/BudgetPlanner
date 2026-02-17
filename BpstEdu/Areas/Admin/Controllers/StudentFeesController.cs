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
    public class StudentFeesController : Controller
    {
        private readonly AppDbContext _context;

        public StudentFeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/StudentFees
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentFees.ToListAsync());
        }

        // GET: Admin/StudentFees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // GET: Admin/StudentFees/Create
        public IActionResult Create(int? studentId)
        {
            var model = new StudentFee();
            if (studentId.HasValue)
            {
                model.StudentId = studentId.Value;
            }
            return View(model);
        }

        // POST: Admin/StudentFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,BatchStudentId,StudentId,SubmittedFeeAmount,Description,FeeSubmittingDate,CreatedDate,LastUpdatedDate")] StudentFee studentFee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentFee);
        }

        // GET: Admin/StudentFees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee == null)
            {
                return NotFound();
            }
            return View(studentFee);
        }

        // POST: Admin/StudentFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UniqueId,BatchStudentId,StudentId,SubmittedFeeAmount,Description,FeeSubmittingDate,CreatedDate,LastUpdatedDate")] StudentFee studentFee)
        {
            if (id != studentFee.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentFeeExists(studentFee.UniqueId))
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
            return View(studentFee);
        }

        // GET: Admin/StudentFees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // POST: Admin/StudentFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee != null)
            {
                _context.StudentFees.Remove(studentFee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentFeeExists(int id)
        {
            return _context.StudentFees.Any(e => e.UniqueId == id);
        }
    }
}
