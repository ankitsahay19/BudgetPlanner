using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using Bpst.API.ViewModels;
using AutoMapper;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/expenseplans")]
    [ApiController]
    [Authorize]
    public class ExpensePlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Bpst.API.Services.UserAccount.IUserAccountService _userAccountService;
        private readonly AutoMapper.IMapper _mapper;

        public ExpensePlansController(AppDbContext context, Bpst.API.Services.UserAccount.IUserAccountService userAccountService, AutoMapper.IMapper mapper)
        {
            _context = context;
            _userAccountService = userAccountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpensePlanDto>>> GetAll()
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var data = await _context.ExpensePlans
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.ParentId)
                .ToListAsync();
            data.ForEach(p => { p.SubExpensePlans = data.Where(sp => sp.ParentId == p.UniqueId).ToList(); });
            var dto = _mapper.Map<IEnumerable<ExpensePlanDto>>(data);
            return Ok(dto);
        }

        [HttpGet("{id}", Name = "GetExpensePlan")]
        public async Task<ActionResult<ExpensePlanDto>> GetById(int id)
        {
            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var plan = await _context.ExpensePlans.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));
            if (plan == null) return NotFound();

            var dto = _mapper.Map<ExpensePlanDto>(plan);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpensePlanDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var entity = _mapper.Map<BudgetPlannerApplication_2025.Models.ExpensePlan>(dto);
            entity.UserId = userId;
            entity.CreatedDate = DateTime.UtcNow;
            if (entity.ParentId == 0) entity.ParentId = null;

            _context.ExpensePlans.Add(entity);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<ExpensePlanDto>(entity);
            return CreatedAtRoute("GetExpensePlan", new { id = createdDto.UniqueId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpensePlanDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var userId = _userAccountService.GetLoggedInUserId();
            if (userId == null) return Unauthorized();

            var existing = await _context.ExpensePlans.AsNoTracking().FirstOrDefaultAsync(c => c.UniqueId == id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            var toUpdate = _mapper.Map<BudgetPlannerApplication_2025.Models.ExpensePlan>(dto);
            toUpdate.UniqueId = id;
            toUpdate.UserId = userId;
            toUpdate.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(toUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<ExpensePlanDto>(toUpdate);
            return Ok(updatedDto);
        }
    }

}
