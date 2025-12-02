
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishListsController(AppDbContext context)
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

        // GET: api/WishLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishList>>> GetWishLists()
        {
            var userId = GetLoggedInUserId();
            return await _context.WishLists.Where(c => c.UserId.Equals(userId)).ToListAsync();
        }

        // GET: api/WishLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WishList>> GetWishList(int id)
        {
            var userId = GetLoggedInUserId();
            var wishList = await _context.WishLists.FirstOrDefaultAsync(c => c.UniqueId == id && (userId == null || c.UserId == userId));
            if (wishList == null)
                return NotFound();
            return Ok(wishList);
        }



        [HttpPost("Create")]
        public async Task<IActionResult> CreateWishList([FromBody] WishList wishList)
        {
            if (!ModelState.IsValid)
                return BadRequest("Payload is required.");

            if (wishList == null)
                return BadRequest("Invalid wish list data.");
            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            wishList.UserId = userId;

            _context.WishLists.Add(wishList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWishList), new { id = wishList.UniqueId }, wishList);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> UpdateWishList(int id, [FromBody] WishList wishList)
        {
            if (wishList == null || id != wishList.UniqueId)
                return BadRequest("Invalid data or ID mismatch.");

            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized();

            var existing = await _context.WishLists.AsNoTracking().FirstOrDefaultAsync(w => w.UniqueId == id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Forbid();

            wishList.LastUpdatedDate = DateTime.UtcNow;
            _context.Entry(wishList).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(wishList);
        }


        //// DELETE: api/WishLists/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWishList(int id)
        //{
        //    var wishList = await _context.WishLists.FindAsync(id);
        //    if (wishList == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.WishLists.Remove(wishList);
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

            var wishList = await _context.WishLists
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (wishList == null)
            {
                return NotFound();
            }

            _context.WishLists.Remove(wishList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishListExists(int id)
        {
            return _context.WishLists.Any(e => e.UniqueId == id);
        }
    }
}
