using AutoMapper;
using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using Bpst.API.Services.IncomeSources;
using Bpst.API.ViewModels;
using BudgetPlannerApi.DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetPlannerApi.Controllers
{
    // Use plural resource name for RESTful routes
    [Route("api/incomesources")]
    [ApiController]
    [Authorize]
    public class IncomeSourcesController : ControllerBase
    {
        private readonly IIncomeSourceService _incomeSourceService;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;
        public IncomeSourcesController(IIncomeSourceService incomeSourceService, IUserAccountService userAccountService, IMapper mapper)
        {
            _incomeSourceService = incomeSourceService;
            _userAccountService = userAccountService;
            _mapper = mapper;
        }

        // GET: api/incomesources
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IncomeSourceDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<IncomeSourceDto>>> GetAll()
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null) return Unauthorized();

            var entities = await _incomeSourceService.GetAllForUserAsync(loggedInUserId.Value);
            var dto = _mapper.Map<IEnumerable<IncomeSourceDto>>(entities);
            return Ok(dto);
        }

        // GET: api/incomesources/{id}
        [HttpGet("{id}", Name = "GetIncomeSource")]
        [ProducesResponseType(typeof(IncomeSourceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IncomeSourceDto>> GetById(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null) return Unauthorized();

            var entity = await _incomeSourceService.GetByIdForUserAsync(id, loggedInUserId.Value);
            if (entity == null) return NotFound();

            var dto = _mapper.Map<IncomeSourceDto>(entity);
            return Ok(dto);
        }

        // POST: api/incomesources
        [HttpPost]
        [ProducesResponseType(typeof(IncomeSourceDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] IncomeSourceDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null) return Unauthorized();

            var toCreate = _mapper.Map<IncomeSource>(dto);
            var created = await _incomeSourceService.CreateAsync(toCreate, loggedInUserId.Value);
            var createdDto = _mapper.Map<IncomeSourceDto>(created);

            // Return 201 Created with Location header pointing to GET by id
            return CreatedAtRoute("GetIncomeSource", new { id = createdDto.UniqueId }, createdDto);
        }

        // PUT: api/incomesources/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IncomeSourceDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] IncomeSourceDto dto)
        {
            if (dto == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null) return Unauthorized();

            try
            {
                var toUpdate = _mapper.Map<IncomeSource>(dto);
                toUpdate.UniqueId = id; // ensure path id wins
                var updated = await _incomeSourceService.UpdateAsync(toUpdate, loggedInUserId.Value);
                var updatedDto = _mapper.Map<IncomeSourceDto>(updated);
                return Ok(updatedDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // DELETE: api/incomesources/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(int id)
        {
            var loggedInUserId = _userAccountService.GetLoggedInUserId();
            if (loggedInUserId == null) return Unauthorized();

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
