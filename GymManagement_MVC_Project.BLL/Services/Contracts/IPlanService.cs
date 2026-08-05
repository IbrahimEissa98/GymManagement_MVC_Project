using GymManagement_MVC_Project.BLL.DTOs.Plan;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IPlanService
{
    Task<IReadOnlyList<PlanIndexDto>> GetAllAsync(CancellationToken ct = default);
    Task<PlanIndexDto?> GetDetailsAsync(int id,CancellationToken ct = default);
    Task<PlanEditDto?> GetForEditAsync(int id, CancellationToken ct = default);
    Task<bool> EditAsync(int id, PlanEditDto editDto, CancellationToken ct = default);
    Task<bool> ToggleActivationAsync(int id, CancellationToken ct = default);
}
