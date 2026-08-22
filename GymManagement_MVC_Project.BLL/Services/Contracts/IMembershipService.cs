using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Membership;
using GymManagement_MVC_Project.BLL.DTOs.Membership.Lookup;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IMembershipService
{
    Task<Result<IReadOnlyList<MembershipIndexDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<MemberLookupItem>>> GetAllUnsubscribedMembersAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<PlanLookupItem>>> GetAllActivePlansAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(MembershipCreateDto createDto, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}
