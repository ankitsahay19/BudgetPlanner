using BudgetPlannerApi.DB.Models;

namespace Bpst.API.Services.IncomeSources
{
    public interface IIncomeSourceService
    {
        Task<IEnumerable<IncomeSource>> GetAllForUserAsync(int userId);
        Task<IncomeSource?> GetByIdForUserAsync(int id, int userId);
        Task<IncomeSource> CreateAsync(IncomeSource incomeSource, int userId);
        Task<IncomeSource> UpdateAsync(IncomeSource incomeSource, int userId);
        Task DeleteAsync(int id, int userId);
        Task<bool> ExistsAsync(int id);
    }
}
