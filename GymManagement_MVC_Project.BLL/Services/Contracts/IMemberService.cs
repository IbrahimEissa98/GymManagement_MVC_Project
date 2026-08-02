using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IMemberService
{
    Task<IReadOnlyList<MemberIndexDto>> GetAllAsync(CancellationToken ct = default);
    Task<bool> CreateAsync(MemberCreateDto createDto, CancellationToken ct = default);
    Task<MemberDetailsDto?> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<HealthRecordDetailsDto?> GetHealthRecordAsync(int id, CancellationToken ct = default);
}
