using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Bpst.API.DB;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensePlanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpensePlanController(AppDbContext context)
        {
            _context = context;
        }

        // Helper to get logged-in user id from claims
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpensePlan>>> GetExpensePlans()
        {
            var userId = GetLoggedInUserId();
            var data = await _context.ExpensePlans
                //  .Include(sc=>sc.ParentId)
                .Where(c => c.UserId.Equals(userId))
                .OrderBy(c => c.ParentId)
                .ToListAsync();
            return Ok(data);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpensePlan>> GetExpensePlan(int id)
        {
            var userId = GetLoggedInUserId();
            var ExpensePlan = await _context.ExpensePlans.Where(c => c.UserId == userId).FirstOrDefaultAsync();

            if (ExpensePlan == null)
            {
                return NotFound();
            }

            return ExpensePlan;
        }

        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEditExpensePlan(ExpensePlan ExpensePlan)
        {
            if (ExpensePlan == null)
                return BadRequest("Invalid ExpensePlan data.");
            else if (ExpensePlan.ParentId == 0)
                ExpensePlan.ParentId = null;

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            ExpensePlan.UserId = userId;

            var existingExpensePlan = await _context.ExpensePlans
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UniqueId == ExpensePlan.UniqueId);

            if (existingExpensePlan == null)
            {
                ExpensePlan.CreatedDate = DateTime.UtcNow;
                _context.ExpensePlans.Add(ExpensePlan);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetExpensePlan), new { id = ExpensePlan.UniqueId }, ExpensePlan);
            }
            else if (userId == existingExpensePlan.UserId)
            {
                ExpensePlan.LastUpdatedDate = DateTime.UtcNow;
                _context.Entry(ExpensePlan).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensePlanExists(ExpensePlan.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(ExpensePlan);
            }
            else
            {
                return Forbid("You do not have permission to edit this Expense Plan.");
            }
        }

        //// DELETE: api/Categories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteExpense Plan(int id)
        //{
        //    var Expense Plan = await _context.Categories.FindAsync(id);
        //    if (Expense Plan == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(Expense Plan);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpensePlan(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var ExpensePlan = await _context.ExpensePlans
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (ExpensePlan == null)
            {
                return NotFound();
            }

            _context.ExpensePlans.Remove(ExpensePlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ExpensePlanExists(int id)
        {
            return _context.ExpensePlans.Any(e => e.UniqueId == id);
        }
    }

}
