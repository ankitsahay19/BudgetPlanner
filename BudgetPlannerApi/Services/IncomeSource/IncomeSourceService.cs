using BudgetPlannerApi.DB.Models;
using Bpst.API.DB;
using Bpst.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bpst.API.Services.IncomeSources
{
    public class IncomeSourceService : IIncomeSourceService
    {
        private readonly IRepository<IncomeSource> _repo;
        private readonly AppDbContext _db;

        public IncomeSourceService(IRepository<IncomeSource> repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<IncomeSource> CreateAsync(IncomeSource incomeSource, int userId)
        {
            incomeSource.UserId = userId;
            incomeSource.CreatedDate = DateTime.UtcNow;
            await _repo.AddAsync(incomeSource);
            await _db.SaveChangesAsync();
            return incomeSource;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var existing = await _db.IncomeSource.FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
            if (existing == null) throw new KeyNotFoundException("IncomeSource not found or not owned by user");
            await _repo.RemoveAsync(existing);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repo.AnyAsync(e => EF.Property<int>(e, "UniqueId") == id);
        }

        public async Task<IEnumerable<IncomeSource>> GetAllForUserAsync(int userId)
        {
            return await _repo.GetAllAsync(w => EF.Property<int?>(w, "UserId") == userId);
        }

        public async Task<IncomeSource?> GetByIdForUserAsync(int id, int userId)
        {
            return await _db.IncomeSource.AsNoTracking().FirstOrDefaultAsync(w => w.UniqueId == id && w.UserId == userId);
        }

        public async Task<IncomeSource> UpdateAsync(IncomeSource incomeSource, int userId)
        {
            var existing = await _db.IncomeSource.AsNoTracking().FirstOrDefaultAsync(w => w.UniqueId == incomeSource.UniqueId);
            if (existing == null) throw new KeyNotFoundException("IncomeSource not found");
            if (existing.UserId != userId) throw new UnauthorizedAccessException();

            incomeSource.LastUpdatedDate = DateTime.UtcNow;
            await _repo.UpdateAsync(incomeSource);
            await _db.SaveChangesAsync();
            return incomeSource;
        }
    }
}
