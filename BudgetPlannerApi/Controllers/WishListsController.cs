using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;
using Bpst.API.Services.WishLists;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListsController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        // GET: api/WishLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetPlannerApplication_2025.Models.WishList>>> GetWishLists()
        {
            var userId = GetLoggedInUserId();
            if (userId == null) return Unauthorized();
            var data = await _wishListService.GetAllForUserAsync(userId.Value);
            return Ok(data);
        }

        // GET: api/WishLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetPlannerApplication_2025.Models.WishList>> GetWishList(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null) return Unauthorized();
            var wishList = await _wishListService.GetByIdForUserAsync(id, userId.Value);
            if (wishList == null)
                return NotFound();
            return Ok(wishList);
        }



        [HttpPost]
        public async Task<IActionResult> CreateWishList([FromBody] BudgetPlannerApplication_2025.Models.WishList wishList)
        {
            if (!ModelState.IsValid)
                return BadRequest("Payload is required.");

            if (wishList == null)
                return BadRequest("Invalid wish list data.");
            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");

            var created = await _wishListService.CreateAsync(wishList, userId.Value);

            return CreatedAtAction(nameof(GetWishList), new { id = created.UniqueId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishList(int id, [FromBody] BudgetPlannerApplication_2025.Models.WishList wishList)
        {
            if (wishList == null || id != wishList.UniqueId)
                return BadRequest("Invalid data or ID mismatch.");

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized();

            try
            {
                var updated = await _wishListService.UpdateAsync(wishList, userId.Value);
                return Ok(updated);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                await _wishListService.DeleteAsync(id, userId.Value);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        private bool WishListExists(int id)
        {
            return _wishListService.ExistsAsync(id).GetAwaiter().GetResult();
        }
    }
}
