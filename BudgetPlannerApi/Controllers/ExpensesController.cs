using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;
using Bpst.API.Services.UserAccount;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public readonly IUserAccountService _userAccountService;

        public ExpensesController(AppDbContext context, IUserAccountService userAccountService)
        {
            _context = context;
            _userAccountService = userAccountService;
        }
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }
        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            var userId = GetLoggedInUserId();

            return await _context.Expenses.Where(c => c.UserId.Equals(userId)).ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var userId = GetLoggedInUserId();
            var expense = await _context.Expenses.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));
            if (expense == null)
                return NotFound();
            return Ok(expense);
        }



        [HttpPost("Create")]
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (expense == null)
                return BadRequest("Invalid expense data.");

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            expense.UserId = userId;

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpense), new { id = expense.UniqueId }, expense);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (expense == null || id != expense.UniqueId)
                return BadRequest("Invalid data or ID mismatch.");

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized();

            var existing = await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.UniqueId == id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Forbid();

            expense.LastUpdatedDate = DateTime.UtcNow;
            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(expense);
        }


        //// DELETE: api/Expenses/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteExpense(int id)
        //{
        //    var expense = await _context.Expenses.FindAsync(id);
        //    if (expense == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Expenses.Remove(expense);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.UniqueId == id);
        }
    }
}
