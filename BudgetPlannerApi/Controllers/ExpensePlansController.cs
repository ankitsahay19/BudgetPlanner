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
using Bpst.API.Services.UserAccount;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensePlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        public readonly IUserAccountService _userAccountService;

        public ExpensePlansController(AppDbContext context, IUserAccountService userAccountService)
        {
            _context = context;
            _userAccountService = userAccountService;
        }

        // Helper to get logged-in user id from claims
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        // GET: api/ExpensePlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpensePlan>>> GetExpensePlans()
        {
            var userId = GetLoggedInUserId();
            var data = await _context.ExpensePlans
                //  .Include(sc=>sc.ParentId)
                .Where(c => c.UserId.Equals(userId))
                .OrderBy(c => c.ParentId)
                .ToListAsync();
            data.ForEach(p => { p.SubExpensePlans = data.Where(sp => sp.ParentId == p.UniqueId).ToList(); });
            return Ok(data);
        }

        // GET: api/ExpensePlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpensePlan>> GetExpensePlan(int id)
        {
            var userId = GetLoggedInUserId();
            var plan = await _context.ExpensePlans.Where(c => c.UserId == userId).FirstOrDefaultAsync();

            if (plan == null)
                return NotFound();

            return plan;
        }

        // [HttpPost("CreateOrEdit")]
        // public async Task<IActionResult> CreateOrEditExpensePlan(ExpensePlan category)
        // {
        //     if (category == null)
        //         return BadRequest("Invalid category data.");
        //     else if (category.ParentId == 0)
        //         category.ParentId = null;

        //     var userId = GetLoggedInUserId();
        //     if (userId == null)
        //         return Unauthorized("User ID not found in token.");
        //     category.UserId = userId;

        //     var existingCategory = await _context.ExpensePlans
        //         .AsNoTracking()
        //         .FirstOrDefaultAsync(c => c.UniqueId == category.UniqueId);

        //     if (existingCategory == null)
        //     {
        //         category.CreatedDate = DateTime.UtcNow;
        //         _context.ExpensePlans.Add(category);
        //         await _context.SaveChangesAsync();

        //         return CreatedAtAction(nameof(GetExpensePlan), new { id = category.UniqueId }, category);
        //     }
        //     else if (userId == existingCategory.UserId)
        //     {
        //         category.LastUpdatedDate = DateTime.UtcNow;
        //         _context.Entry(category).State = EntityState.Modified;

        //         try
        //         {
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!ExpensePlanExists(category.UniqueId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }

        //         return Ok(category);
        //     }
        //     else
        //     {
        //         return Forbid("You do not have permission to edit this category.");
        //     }
        // }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpensePlan(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var category = await _context.ExpensePlans
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (category == null)
            {
                return NotFound();
            }

            _context.ExpensePlans.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ExpensePlanExists(int id)
        {
            return _context.ExpensePlans.Any(e => e.UniqueId == id);
        }



        [HttpPost("Create")]
        public async Task<IActionResult> CreateIncome([FromBody] ExpensePlan plan)
        {
            if (plan == null) return BadRequest("Invalid Plan data.");
            if (_userAccountService.GetLoggedInUserId() == null) return Unauthorized("User ID not found in token.");
            plan.UserId = _userAccountService.GetLoggedInUserId();
            plan.CreatedDate = DateTime.UtcNow;
            _context.ExpensePlans.Add(plan);
            await _context.SaveChangesAsync();
            return Ok(plan);
        }


        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditIncome(int id, [FromBody] ExpensePlan plan)
        {
            if (plan == null || id != plan.UniqueId) return BadRequest("Invalid plan data or ID mismatch.");
            var existingPlan = await _context.ExpensePlans.AsNoTracking().FirstOrDefaultAsync(c => c.UniqueId == id);
            if (existingPlan == null) return NotFound("plan not found.");

            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (existingPlan.UserId != loggedInUserId) return Forbid("You do not have permission to edit this income source.");
            existingPlan.Name = plan.Name;
            existingPlan.Description = plan.Name;
            existingPlan.AllocatedAmount = plan.AllocatedAmount;
            existingPlan.ParentId = plan.ParentId;
            existingPlan.Year = plan.Year;
            existingPlan.Month = plan.Month;
            existingPlan.LastUpdatedDate = DateTime.UtcNow;
            _context.Entry(existingPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(existingPlan);
        }

    }

}
