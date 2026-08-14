using GymManagement_MVC_Project.BLL.DTOs.Membership;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.BLL.Extensions.Mapping;

public static class MembershipDtoMapping
{
    public static MembershipIndexDto GetIndexDto(this MembershipIndexDtoQS dtoQS)
    {
        return new MembershipIndexDto
        {
            Id = dtoQS.Id,
            MemberName = dtoQS.MemberName,
            PlanName = dtoQS.PlanName,
            StartDate = dtoQS.StartDate,
            EndDate = dtoQS.EndDate,
        };
    }

    public static Membership GetMembership(this MembershipCreateDto createDto, IDateTimeProvider clock, int planDuration)
    {
        return new Membership
        {
            MemberId = createDto.MemberId,
            PlanId = createDto.PlanId,
            StartDate = clock.UtcNow,
            EndDate = clock.UtcNow.AddDays(planDuration),
        };
    }
}
