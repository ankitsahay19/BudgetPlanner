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
using Bpst.API.Services.IncomeSources;

namespace BudgetPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeSourceController : ControllerBase
    {
        private readonly IIncomeSourceService _incomeSourceService;
        public readonly IUserAccountService _userAccountService;

        public IncomeSourceController(IIncomeSourceService incomeSourceService, IUserAccountService userAccountService)
        {
            _incomeSourceService = incomeSourceService;
            _userAccountService = userAccountService;
        }
        // GET: api/IncomeSource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeSource>>> GetIncomeSource()
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return Unauthorized();
            var data = await _incomeSourceService.GetAllForUserAsync(loggedInUserId.Value);
            return Ok(data);
        }

        // GET: api/IncomeSource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeSource>> GetIncomeSource(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return Unauthorized();
            var incomeSource = await _incomeSourceService.GetByIdForUserAsync(id, loggedInUserId.Value);

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

            var created = await _incomeSourceService.CreateAsync(incomeSource, loggedInUserId.Value);

            return CreatedAtAction(nameof(GetIncomeSource), new { id = created.UniqueId }, created);
        }

        // PUT: api/IncomeSource/5
          [HttpPut("Edit/{id}")]
         public async Task<IActionResult> UpdateIncomeSource(int id, [FromBody] IncomeSource incomeSource)
        {
            if (incomeSource == null || id != incomeSource.UniqueId)
                return BadRequest("Invalid IncomeSource data or ID mismatch.");

            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return Unauthorized();

            try
            {
                var updated = await _incomeSourceService.UpdateAsync(incomeSource, loggedInUserId.Value);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Income source not found.");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("You do not have permission to edit this income source.");
            }
        }

        // DELETE: api/IncomeSource/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomeSource(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null)
                return Unauthorized();

            try
            {
                await _incomeSourceService.DeleteAsync(id, loggedInUserId.Value);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
