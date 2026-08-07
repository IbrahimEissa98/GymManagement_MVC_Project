using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class PlanService(IUnitOfWork unitOfWork) : IPlanService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IReadOnlyList<PlanIndexDto>> GetAllAsync(CancellationToken ct = default)
    {
        var plans = await _unitOfWork.Plans.GetAllAsync(ct);

        return [..plans.Select(p => new PlanIndexDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            DurationDays = p.DurationDays,
            IsActive = p.IsActive
        })];

    }

    public async Task<PlanIndexDto?> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdAsync(id, ct);

        if (plan is null) return null;

        var planDto = new PlanIndexDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive
        };

        return planDto;
    }

    public async Task<PlanEditDto?> GetForEditAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null || !plan.IsActive || plan.Memberships.Any(m => m.EndDate > DateTime.UtcNow))
            return null;

        var editDto = new PlanEditDto
        {
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive
        };

        return editDto;
    }

    public async Task<bool> EditAsync(int id, PlanEditDto editDto, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null || !plan.IsActive || plan.Memberships.Any(m => m.EndDate > DateTime.UtcNow))
            return false;

        plan.Price = editDto.Price;
        plan.Description = editDto.Description;
        plan.DurationDays = editDto.DurationDays;

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0; ;
    }

    public async Task<bool> ToggleActivationAsync(int id, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.Plans.GetByIdWithIncludesAsync(id, ct: ct, includes: p => p.Memberships);

        if (plan is null || plan.Memberships.Any(m => m.EndDate > DateTime.UtcNow))
            return false;

        plan.IsActive = !plan.IsActive;

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0;
    }
}
