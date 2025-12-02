using BudgetPlannerApplication_2025.Models;

namespace Bpst.API.Services.WishLists
{
    public interface IWishListService
    {
        Task<IEnumerable<WishList>> GetAllForUserAsync(int userId);
        Task<WishList?> GetByIdForUserAsync(int id, int userId);
        Task<WishList> CreateAsync(WishList wishList, int userId);
        Task<WishList> UpdateAsync(WishList wishList, int userId);
        Task DeleteAsync(int id, int userId);
        Task<bool> ExistsAsync(int id);
    }
}
