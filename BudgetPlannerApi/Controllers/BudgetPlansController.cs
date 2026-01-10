using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;

namespace BudgetPlannerApplication_2025.Controllers
{
    using Bpst.API.ViewModels;
    using AutoMapper;

    [Route("api/budgetplans")]
    [ApiController]
    public class BudgetPlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Bpst.API.Services.UserAccount.IUserAccountService _userAccountService;
        private readonly AutoMapper.IMapper _mapper;

        public BudgetPlansController(AppDbContext context, Bpst.API.Services.UserAccount.IUserAccountService userAccountService, AutoMapper.IMapper mapper)
        {
            _context = context;
            _userAccountService = userAccountService;
            _mapper = mapper;
        }

        // GET: api/budgetplans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetPlanDto>>> GetAll()
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var items = await _context.BudgetPlans.Where(c => c.UserId == userId).ToListAsync();
            var dto = _mapper.Map<IEnumerable<BudgetPlanDto>>(items);
            return Ok(dto);
        }

        // GET: api/budgetplans/{id}
        [HttpGet("{id}", Name = "GetBudgetPlan")]
        public async Task<ActionResult<BudgetPlanDto>> GetById(int id)
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var budgetPlan = await _context.BudgetPlans.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));
            if (budgetPlan == null) return NotFound();

            var dto = _mapper.Map<BudgetPlanDto>(budgetPlan);
            return Ok(dto);
        }

        // POST: api/budgetplans
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetPlanDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized("User ID not found in token.");

            var entity = _mapper.Map<BudgetPlannerApplication_2025.Models.BudgetPlan>(dto);
            entity.UserId = userId;
            entity.LastUpdatedDate = DateTime.UtcNow;

            _context.BudgetPlans.Add(entity);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<BudgetPlanDto>(entity);
            return CreatedAtRoute("GetBudgetPlan", new { id = createdDto.UniqueId }, createdDto);
        }

        // PUT: api/budgetplans/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BudgetPlanDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var existing = await _context.BudgetPlans.AsNoTracking().FirstOrDefaultAsync(b => b.UniqueId == id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            var toUpdate = _mapper.Map<BudgetPlannerApplication_2025.Models.BudgetPlan>(dto);
            toUpdate.UniqueId = id;
            toUpdate.UserId = userId;
            toUpdate.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(toUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<BudgetPlanDto>(toUpdate);
            return Ok(updatedDto);
        }

        // DELETE: api/budgetplans/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var budgetPlan = await _context.BudgetPlans.FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
            if (budgetPlan == null) return NotFound();

            _context.BudgetPlans.Remove(budgetPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
