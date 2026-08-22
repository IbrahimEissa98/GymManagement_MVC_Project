using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Membership;
using GymManagement_MVC_Project.BLL.DTOs.Membership.Lookup;
using GymManagement_MVC_Project.BLL.Extensions.Mapping;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MembershipService(
    IUnitOfWork unitOfWork,
    IMembershipQueryService queryService,
    IDateTimeProvider clock) : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMembershipQueryService _queryService = queryService;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<MembershipIndexDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var memberships = await _queryService.GetAllAsync(_clock.Today, ct);

        return Result<IReadOnlyList<MembershipIndexDto>>.Success(
            [.. memberships.Select(ms => ms.GetIndexDto())]);
    }

    public async Task<Result<IReadOnlyList<MemberLookupItem>>> GetAllUnsubscribedMembersAsync(CancellationToken ct = default)
    {
        var members = await _unitOfWork.Members.FindAsync(
            m => m.Memberships.Any(
                ms => ms.IsDeleted == false
                && DateOnly.FromDateTime(ms.EndDate) < _clock.Today) || m.Memberships.Count == 0
            , ct);
        return Result<IReadOnlyList<MemberLookupItem>>.Success(
            [..members.Select(m => new MemberLookupItem
            {
                Id = m.Id,
                Name = m.Name
            })]);
    }

    public async Task<Result<IReadOnlyList<PlanLookupItem>>> GetAllActivePlansAsync(CancellationToken ct = default)
    {
        var plans = await _unitOfWork.Plans.FindAsync(p => p.IsActive == true, ct);
        return Result<IReadOnlyList<PlanLookupItem>>.Success(
            [..plans.Select(p => new PlanLookupItem
            {
                Id = p.Id,
                Name = p.Name
            })]);
    }

    public async Task<Result> CreateAsync(MembershipCreateDto createDto, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(createDto.MemberId, ct: ct);
        if (member is null)
            return Result.Failure("Selected Member not found.", ErrorType.NotFound, nameof(createDto.MemberId));
        var hasActiveMembership = await _queryService.IsMemberHasActiveMembership(createDto.MemberId, ct);
        if (hasActiveMembership)
            return Result.Failure("Member cannot have more than one Active membership at the same time.", ErrorType.Validation, nameof(createDto.MemberId));

        var plan = await _unitOfWork.Plans.GetByIdAsync(createDto.PlanId, ct);
        if (plan is null)
            return Result.Failure("Selected Plan not found.", ErrorType.NotFound, nameof(createDto.PlanId));
        if (plan.IsActive == false)
            return Result.Failure("can not select inactive plan", ErrorType.Validation, nameof(createDto.PlanId));

        var newMembership = createDto.GetMembership(_clock, plan.DurationDays);
        try
        {
            await _unitOfWork.Memberships.AddAsync(newMembership, ct);
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to create membership.", ErrorType.Failure);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create membership, {ex.Message}", ErrorType.Failure);
        }

    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var membership = await _unitOfWork.Memberships.GetByIdAsync(id, ct);
        if (membership is null)
            return Result.Failure("Membership not found", ErrorType.NotFound);
        if (membership.Status == MembershipStatus.Expired)
            return Result.Failure("Can not cancel Expired membership.", ErrorType.Failure);

        try
        {
            _unitOfWork.Memberships.Remove(membership);
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to cancel membership.", ErrorType.Failure);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to cancel membership, {ex.Message}", ErrorType.Failure);
        }
    }
}
