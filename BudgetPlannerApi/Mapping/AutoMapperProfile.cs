using AutoMapper;
using BudgetPlannerApi.DB.Models;
using Bpst.API.ViewModels;
namespace Bpst.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity -> DTO (include timestamps)
            CreateMap<IncomeSource, IncomeSourceDto>();

            // DTO -> Entity: ignore server-controlled fields to avoid overwriting them during create/update
            CreateMap<IncomeSourceDto, IncomeSource>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());

            // BudgetPlan mappings
            CreateMap<BudgetPlannerApplication_2025.Models.BudgetPlan, BudgetPlanDto>();
            CreateMap<BudgetPlanDto, BudgetPlannerApplication_2025.Models.BudgetPlan>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.AppUser, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());

            // ExpensePlan mappings
            CreateMap<BudgetPlannerApplication_2025.Models.ExpensePlan, ExpensePlanDto>();
            CreateMap<ExpensePlanDto, BudgetPlannerApplication_2025.Models.ExpensePlan>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.SubExpenses, opt => opt.Ignore());

            // Expense mappings
            CreateMap<BudgetPlannerApplication_2025.Models.Expense, ExpenseDto>();
            CreateMap<ExpenseDto, BudgetPlannerApplication_2025.Models.Expense>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.AppUser, opt => opt.Ignore())
                .ForMember(dest => dest.ExpensePlan, opt => opt.Ignore());

            // WishList mappings
            CreateMap<BudgetPlannerApplication_2025.Models.WishList, WishListDto>();
            CreateMap<WishListDto, BudgetPlannerApplication_2025.Models.WishList>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.AppUser, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());
        }
    }
}
