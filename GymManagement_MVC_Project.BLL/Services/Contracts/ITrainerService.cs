using GymManagement_MVC_Project.BLL.DTOs.Trainer;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface ITrainerService
{
    Task<IReadOnlyList<TrainerIndexDto>> GetAllAsync(CancellationToken ct = default);
    Task<bool> CreateAsync(TrainerCreateDto createDto, CancellationToken ct = default);
    Task<TrainerDetailsDto?> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<TrainerEditDto?> GetForEditAsync(int id, CancellationToken ct = default);
    Task<bool> EditAsync(int id, TrainerEditDto editDto, CancellationToken ct = default);
    Task<TrainerDeleteDto?> GetForDeleteOrRestoreAsync(int id, bool IsDelete = true, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ActivateAsync(int id, CancellationToken ct = default);
}
