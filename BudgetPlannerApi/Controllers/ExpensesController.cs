using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using Bpst.API.ViewModels;
using AutoMapper;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public ExpensesController(AppDbContext context, IUserAccountService userAccountService, IMapper mapper)
        {
            _context = context;
            _userAccountService = userAccountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var list = await _context.Expenses.Where(c => c.UserId == userId).ToListAsync();
            var dto = _mapper.Map<IEnumerable<ExpenseDto>>(list);
            return Ok(dto);
        }

        [HttpGet("{id}", Name = "GetExpense")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var expense = await _context.Expenses.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));
            if (expense == null) return NotFound();

            var dto = _mapper.Map<ExpenseDto>(expense);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var entity = _mapper.Map<Expense>(dto);
            entity.UserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<ExpenseDto>(entity);
            return CreatedAtRoute("GetExpense", new { id = createdDto.UniqueId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var existing = await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.UniqueId == id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            var toUpdate = _mapper.Map<Expense>(dto);
            toUpdate.UniqueId = id;
            toUpdate.UserId = userId;
            toUpdate.CreatedDate = existing.CreatedDate;
            toUpdate.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(toUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<ExpenseDto>(toUpdate);
            return Ok(updatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var expense = await _context.Expenses.FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
            if (expense == null) return NotFound();

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
