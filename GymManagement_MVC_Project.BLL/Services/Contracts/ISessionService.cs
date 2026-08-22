using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Session;
using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface ISessionService
{
    Task<Result<IReadOnlyList<SessionIndexDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<SessionIndexDto>> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<CategoryLookupItem>>> GetAllCategoryLookupItems(CancellationToken ct = default);
    Task<Result<IReadOnlyList<TrainerLookupItem>>> GetAllTrainersByCategoryId(int categoryId, CancellationToken ct = default);
    Task<Result> CreateAsync(SessionCreateDto createDto, CancellationToken ct = default);
    Task<Result<SessionEditDto>> GetForEditAsync(int sessionId, CancellationToken ct = default);
    Task<Result> EditAsync(int sessionId, SessionEditDto editDto, CancellationToken ct = default);
    Task<Result<SessionDeleteDto>> GetForDeleteOrActivateAsync(int sessionId, bool isDelete = true, CancellationToken ct = default);
    Task<Result> DeleteAsync(int sessionId, CancellationToken ct = default);
    Task<Result> ActivateAsync(int sessionId, CancellationToken ct = default);
}
