using GymManagement_MVC_Project.BLL.DTOs.Membership;
using GymManagement_MVC_Project.PL.ViewModels.Membership;

namespace GymManagement_MVC_Project.PL.Extensions.Mapping;

public static class MembershipVMMapping
{
    public static MembershipIndexViewModel GetViewModel(this MembershipIndexDto indexDto)
    {
        return new MembershipIndexViewModel
        {
            Id = indexDto.Id,
            MemberName = indexDto.MemberName,
            PlanName = indexDto.PlanName,
            StartDate = indexDto.StartDate,
            EndDate = indexDto.EndDate
        };
    }

    public static MembershipCreateDto GetCreateDto(this MembershipCreateViewModel createVM)
    {
        return new MembershipCreateDto
        {
            MemberId = createVM.MemberId,
            PlanId = createVM.PlanId
        };
    }
}
