using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.DAL.Queries.Contracts;

public interface IMembershipQueryService
{
    Task<IReadOnlyList<MembershipIndexDtoQS>> GetAllAsync(DateOnly today, CancellationToken ct = default);
    Task<bool> IsMemberHasActiveMembership(int memberId, CancellationToken ct = default);
}
