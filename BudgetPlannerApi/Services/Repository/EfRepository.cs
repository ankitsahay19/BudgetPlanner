using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Bpst.API.DB;

namespace Bpst.API.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;

        public EfRepository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public IQueryable<T> Query() => _set;

        public async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _set.AsNoTracking().ToListAsync();
            return await _set.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            return entity;
        }

        public async Task RemoveAsync(T entity)
        {
            _set.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.AsNoTracking().AnyAsync(predicate);
        }
    }
}
