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
            var incomeSource = await _context.IncomeSource.FindAsync(id);

            if (incomeSource == null)
            {
                return NotFound();
            }

            return incomeSource;
        }





        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEditIncome(IncomeSource IncomeSource)
        {
            if (IncomeSource == null)
                return BadRequest("Invalid IncomeSource data.");

            var existingIncome = await _context.IncomeSource.AsNoTracking().FirstOrDefaultAsync(c => c.UniqueId == IncomeSource.UniqueId);
            if (existingIncome != null && existingIncome.UniqueId == IncomeSource.UniqueId)
            {
                if (_userAccountService.GetLoggedInUserId() != existingIncome.UserId && existingIncome.UserId != IncomeSource.UserId)
                    return Forbid("You do not have permission to edit this category.");
                else
                {
                    existingIncome.LastUpdatedDate = DateTime.UtcNow;
                    existingIncome.SourceName = IncomeSource.SourceName;
                    existingIncome.IncomeAmount = IncomeSource.IncomeAmount;
                    _context.Entry(existingIncome).State = EntityState.Modified;
                }
            }
            else if (existingIncome == null)
            {
                IncomeSource.UserId ??= _userAccountService.GetLoggedInUserId();
                IncomeSource.CreatedDate = DateTime.UtcNow;
                _context.IncomeSource.Add(IncomeSource);

            }
            await _context.SaveChangesAsync();
            return Ok(IncomeSource);
        } 

        // DELETE: api/IncomeSource/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomeSource(int id)
        {
            var incomeSource = await _context.IncomeSource.FindAsync(id);
            if (incomeSource == null)
            {
                return NotFound();
            }

            _context.IncomeSource.Remove(incomeSource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncomeSourceExists(int id)
        {
            return _context.IncomeSource.Any(e => e.UniqueId == id);
        }
    }
}
