using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IMemberService
{
    Task<Result<IReadOnlyList<MemberIndexDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(MemberCreateDto createDto, CancellationToken ct = default);
    Task<Result<MemberDetailsDto>> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<Result<HealthRecordDetailsDto>> GetHealthRecordAsync(int id, CancellationToken ct = default);
    Task<Result<MemberToUpdateDto>> GetForUpdateAsync(int id, CancellationToken ct = default);
    Task<Result> UpdateAsync(int id, MemberToUpdateDto updateDto, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result<MemberDeleteDto>> GetForActivateAsync(int id, CancellationToken ct = default);
    Task<Result> ActivateAsync(int id, CancellationToken ct = default);
}
