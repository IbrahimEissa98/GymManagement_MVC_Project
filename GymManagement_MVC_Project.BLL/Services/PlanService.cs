using AutoMapper;
using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class PlanService(IUnitOfWork unitOfWork, IDateTimeProvider clock, IMapper mapper) : IPlanService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<PlanIndexDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var plans = await _unitOfWork.Plans.GetAllAsync(ct);

        return Result<IReadOnlyList<PlanIndexDto>>.Success(
                    _mapper.Map<IReadOnlyList<PlanIndexDto>>(plans));
    }

    public async Task<Result<PlanIndexDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdAsync(id, ct);

        if (plan is null) return Result<PlanIndexDto>.Failure("Plan not found.", ErrorType.NotFound);

        var planDto = _mapper.Map<PlanIndexDto>(plan);

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

        var editDto = _mapper.Map<PlanEditDto>(plan);

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
        if (plan.Name != editDto.Name)
            return Result.Failure("Can not edit plan name.", ErrorType.Validation);

        //plan.Price = editDto.Price;
        //plan.Description = editDto.Description;
        //plan.DurationDays = editDto.DurationDays;

        var editedPlan = _mapper.Map(editDto, plan);

        //_unitOfWork.Plans.Update(editedPlan);

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
