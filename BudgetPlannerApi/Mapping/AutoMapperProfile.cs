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
        }
    }
}
