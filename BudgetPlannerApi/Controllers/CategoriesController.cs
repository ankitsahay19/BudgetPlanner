using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlannerApplication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Bpst.API.DB;

namespace BudgetPlannerApplication_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // Helper to get logged-in user id from claims
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var userId = GetLoggedInUserId();
            var data = await _context.Categories
              //  .Include(sc=>sc.ParentId)
                .Where(c => c.UserId.Equals(userId))
                .OrderBy(c=>c.ParentId)
                .ToListAsync();
            return Ok(data);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var userId = GetLoggedInUserId();
            var category = await _context.Categories.Where(c => c.UserId == userId).FirstOrDefaultAsync();
             
            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEditCategory(Category category)
        {
            if (category == null)
                return BadRequest("Invalid category data.");
            else if (category.ParentId == 0)
                category.ParentId = null;

                var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            category.UserId = userId;

            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UniqueId == category.UniqueId);

            if (existingCategory == null)
            {
                category.CreatedDate = DateTime.UtcNow;
                 _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = category.UniqueId }, category);
            }
            else if (userId == existingCategory.UserId)
            {
                category.LastUpdatedDate = DateTime.UtcNow;
                _context.Entry(category).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(category);
            }
            else
            {
                return Forbid("You do not have permission to edit this category.");
            }
        }

        //// DELETE: api/Categories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(category);
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

            var category = await _context.Categories
                .FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.UniqueId == id);
        }
    }
    
}
