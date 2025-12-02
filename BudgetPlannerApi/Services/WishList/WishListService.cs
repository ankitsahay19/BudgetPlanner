using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;
using Bpst.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bpst.API.Services.WishLists
{
    public class WishListService : IWishListService
    {
        private readonly IRepository<WishList> _repo;
        private readonly AppDbContext _db;

        public WishListService(IRepository<WishList> repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<WishList> CreateAsync(WishList wishList, int userId)
        {
            wishList.UserId = userId;
            wishList.LastUpdatedDate = DateTime.UtcNow;
            await _repo.AddAsync(wishList);
            await _db.SaveChangesAsync();
            return wishList;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var existing = await _db.WishLists.FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
            if (existing == null) throw new KeyNotFoundException("WishList not found or not owned by user");
            await _repo.RemoveAsync(existing);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repo.AnyAsync(e => EF.Property<int>(e, "UniqueId") == id);
        }

        public async Task<IEnumerable<WishList>> GetAllForUserAsync(int userId)
        {
            return await _repo.GetAllAsync(w => EF.Property<int?>(w, "UserId") == userId);
        }

        public async Task<WishList?> GetByIdForUserAsync(int id, int userId)
        {
            return await _db.WishLists.AsNoTracking().FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
        }

        public async Task<WishList> UpdateAsync(WishList wishList, int userId)
        {
            var existing = await _db.WishLists.AsNoTracking().FirstOrDefaultAsync(w => w.UniqueId == wishList.UniqueId);
            if (existing == null) throw new KeyNotFoundException("WishList not found");
            if (existing.UserId != userId) throw new UnauthorizedAccessException();

            wishList.LastUpdatedDate = DateTime.UtcNow;
            await _repo.UpdateAsync(wishList);
            await _db.SaveChangesAsync();
            return wishList;
        }
    }
}
