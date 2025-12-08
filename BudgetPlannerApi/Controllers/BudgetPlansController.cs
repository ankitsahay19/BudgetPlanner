using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetPlansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetPlansController(AppDbContext context)
        {
            _context = context;
        }
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }
        // GET: api/BudgetPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetPlan>>> GetBudgetPlans()
        {
            var userId = GetLoggedInUserId();
            return await _context.BudgetPlans.Where(c => c.UserId.Equals(userId)).ToListAsync();
        }

        // GET: api/BudgetPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetPlan>> GetBudgetPlan(int id)
        {
            var userId = GetLoggedInUserId(); 
            var budgetPlan = await _context.BudgetPlans.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));  
            if (budgetPlan == null)
                return NotFound(); 
            return Ok(budgetPlan);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateBudgetPlan([FromBody] BudgetPlan budgetPlan)
        {
            if (budgetPlan == null)
                return BadRequest("Invalid data.");
            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            budgetPlan.UserId = userId;

            _context.BudgetPlans.Add(budgetPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBudgetPlan), new { id = budgetPlan.UniqueId }, budgetPlan);
        }

        // PUT: api/BudgetPlans/{id}
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> UpdateBudgetPlan(int id, [FromBody] BudgetPlan budgetPlan)
        {
            if (budgetPlan == null || id != budgetPlan.UniqueId)
                return BadRequest("Invalid data or ID mismatch.");

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized();

            var existing = await _context.BudgetPlans.AsNoTracking().FirstOrDefaultAsync(b => b.UniqueId == id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Forbid();

            budgetPlan.LastUpdatedDate = DateTime.UtcNow;
            _context.Entry(budgetPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(budgetPlan);
        } 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var budgetPlan = await _context.BudgetPlans
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (budgetPlan == null)
            {
                return NotFound();
            }

            _context.BudgetPlans.Remove(budgetPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetPlanExists(int id)
        {
            return _context.BudgetPlans.Any(e => e.UniqueId == id);
        }
    }
}
