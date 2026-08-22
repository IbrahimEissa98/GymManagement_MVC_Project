using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.PL.ViewModels.Plan;

namespace GymManagement_MVC_Project.PL.Profiles;

public class PlanVMProfile : Profile
{
    public PlanVMProfile()
    {
        CreateMap<PlanIndexDto, PlanIndexViewModel>();

        CreateMap<PlanEditViewModel, PlanEditDto>();
    }
}
