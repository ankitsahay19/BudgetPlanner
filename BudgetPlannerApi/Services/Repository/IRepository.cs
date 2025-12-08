using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Bpst.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
