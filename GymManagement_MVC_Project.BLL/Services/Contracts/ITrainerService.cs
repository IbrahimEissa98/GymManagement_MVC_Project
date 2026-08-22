using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Trainer;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface ITrainerService
{
    Task<Result<IReadOnlyList<TrainerIndexDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(TrainerCreateDto createDto, CancellationToken ct = default);
    Task<Result<TrainerDetailsDto>> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<Result<TrainerEditDto>> GetForEditAsync(int id, CancellationToken ct = default);
    Task<Result> EditAsync(int id, TrainerEditDto editDto, CancellationToken ct = default);
    Task<Result<TrainerDeleteDto>> GetForDeleteOrRestoreAsync(int id, bool IsDelete = true, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result> ActivateAsync(int id, CancellationToken ct = default);
}
