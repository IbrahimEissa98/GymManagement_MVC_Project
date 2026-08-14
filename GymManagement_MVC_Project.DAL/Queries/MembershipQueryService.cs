using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Queries.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Queries;

public class MembershipQueryService(GymDbContext gymDbContext) : IMembershipQueryService
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    public async Task<IReadOnlyList<MembershipIndexDtoQS>> GetAllAsync(DateOnly today, CancellationToken ct = default)
    {
        return [..await _gymDbContext.Memberships
                    .Where(ms => DateOnly.FromDateTime(ms.StartDate) <= today
                                        && DateOnly.FromDateTime(ms.EndDate) >= today)
                    .Select(ms => new MembershipIndexDtoQS
                    {
                        Id = ms.Id,
                        MemberName = ms.Member.Name,
                        PlanName = ms.Plan.Name,
                        StartDate = ms.StartDate,
                        EndDate = ms.EndDate
                    }).ToListAsync(ct)];
    }

    public async Task<bool> IsMemberHasActiveMembership(int memberId, CancellationToken ct = default)
    {
        return await _gymDbContext.Members
                    .AsNoTracking()
                    .Where(m => m.Id == memberId)
                    .AnyAsync(m => m.Memberships
                            .Any(ms => ms.EndDate > DateTime.UtcNow && ms.IsDeleted == false), ct);
    }
}
