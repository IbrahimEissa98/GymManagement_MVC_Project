using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Plan;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IPlanService
{
    Task<Result<IReadOnlyList<PlanIndexDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<PlanIndexDto>> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<Result<PlanEditDto>> GetForEditAsync(int id, CancellationToken ct = default);
    Task<Result> EditAsync(int id, PlanEditDto editDto, CancellationToken ct = default);
    Task<Result> ToggleActivationAsync(int id, CancellationToken ct = default);
}
