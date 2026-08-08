using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class PlanService(IUnitOfWork unitOfWork, IDateTimeProvider clock) : IPlanService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<PlanIndexDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var plans = await _unitOfWork.Plans.GetAllAsync(ct);

        return Result<IReadOnlyList<PlanIndexDto>>.Success([..plans.Select(p => new PlanIndexDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            DurationDays = p.DurationDays,
            IsActive = p.IsActive
        })]);

    }

    public async Task<Result<PlanIndexDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdAsync(id, ct);

        if (plan is null) return Result<PlanIndexDto>.Failure("Plan not found.", ErrorType.NotFound);

        var planDto = new PlanIndexDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive
        };

        return Result<PlanIndexDto>.Success(planDto);
    }

    public async Task<Result<PlanEditDto>> GetForEditAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null)
            return Result<PlanEditDto>.Failure("Plan not found.", ErrorType.NotFound);
        if (!plan.IsActive)
            return Result<PlanEditDto>.Failure("Can not edit inactive plan.", ErrorType.Conflict);
        if (plan.Memberships.Any(m => m.EndDate > _clock.UtcNow))
            return Result<PlanEditDto>.Failure("Can not edit registered plan.", ErrorType.Conflict);

        var editDto = new PlanEditDto
        {
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive
        };

        return Result<PlanEditDto>.Success(editDto);
    }

    public async Task<Result> EditAsync(int id, PlanEditDto editDto, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null)
            return Result.Failure("Plan not found.", ErrorType.NotFound);
        if (!plan.IsActive)
            return Result.Failure("Can not edit inactive plan.", ErrorType.Conflict);
        if (plan.Memberships.Any(m => m.EndDate > _clock.UtcNow))
            return Result.Failure("Can not edit registered plan.", ErrorType.Conflict);

        plan.Price = editDto.Price;
        plan.Description = editDto.Description;
        plan.DurationDays = editDto.DurationDays;

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to edit plan.", ErrorType.Failure);
    }

    public async Task<Result> ToggleActivationAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null)
            return Result.Failure("Plan not found.", ErrorType.NotFound);
        if (plan.Memberships.Any(m => m.EndDate > _clock.UtcNow))
            return Result.Failure("Can not deactivate registered plan.", ErrorType.Conflict);

        plan.IsActive = !plan.IsActive;

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to deactivate the plan.", ErrorType.Failure);
    }
}
