using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using BudgetPlannerApi.DB.Models;
using BudgetPlannerApplication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeSourceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public readonly IUserAccountService _userAccountService;

        public IncomeSourceController(AppDbContext context, IUserAccountService userAccountService)
        {
            _context = context;
            _userAccountService = userAccountService;
        }
        // GET: api/IncomeSource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeSource>>> GetIncomeSource()
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return await _context.IncomeSource.ToListAsync();
            return await _context.IncomeSource.Where(i => i.UserId.Equals(loggedInUserId)).ToListAsync();
        }

        // GET: api/IncomeSource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeSource>> GetIncomeSource(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            var incomeSource = await _context.IncomeSource.FirstOrDefaultAsync(i => i.UniqueId == id && (loggedInUserId == null || i.UserId == loggedInUserId));

            if (incomeSource == null)
                return NotFound();

            return Ok(incomeSource);
        }

        // POST: api/IncomeSource
        [HttpPost("Create")]
        public async Task<IActionResult> CreateIncomeSource([FromBody] IncomeSource incomeSource)
        {
            if (incomeSource == null)
                return BadRequest("Invalid IncomeSource data.");
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return Unauthorized("User ID not found in token.");
            incomeSource.UserId = loggedInUserId;
            incomeSource.CreatedDate = DateTime.UtcNow;

            _context.IncomeSource.Add(incomeSource);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncomeSource), new { id = incomeSource.UniqueId }, incomeSource);
        }

        // PUT: api/IncomeSource/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> UpdateIncomeSource(int id, [FromBody] IncomeSource incomeSource)
        {
            if (incomeSource == null || id != incomeSource.UniqueId)
                return BadRequest("Invalid IncomeSource data or ID mismatch.");

            var existingIncome = await _context.IncomeSource.AsNoTracking()
                .FirstOrDefaultAsync(c => c.UniqueId == id);

            if (existingIncome == null)
                return NotFound("Income source not found.");

            // Check permissions
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (existingIncome.UserId != loggedInUserId)
                return Forbid("You do not have permission to edit this income source.");

            // Update only editable fields
            existingIncome.SourceName = incomeSource.SourceName;
            existingIncome.IncomeAmount = incomeSource.IncomeAmount;
            existingIncome.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(existingIncome).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingIncome);
        }

        // DELETE: api/IncomeSource/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomeSource(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            var incomeSource = await _context.IncomeSource.FirstOrDefaultAsync(i => i.UniqueId == id && (loggedInUserId == null || i.UserId == loggedInUserId));
            if (incomeSource == null)
                return NotFound();

            _context.IncomeSource.Remove(incomeSource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
