using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.BLL.Profiles;

public class PlanDtoProfile : Profile
{
    public PlanDtoProfile()
    {
        CreateMap<Plan, PlanIndexDto>();

        CreateMap<Plan, PlanEditDto>();

        CreateMap<PlanEditDto, Plan>()
            .ForMember(d => d.IsActive, s => s.MapFrom(_ => true));
    }
}
