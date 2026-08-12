using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.DAL.Queries.Contracts;

public interface ISessionQueryService
{
    Task<IReadOnlyList<SessionIndexDtoQS>> GetAllSessionsIndexQSAsync(CancellationToken ct = default);
    Task<SessionIndexDtoQS?> GetSessionDetailsQSAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<TrainerLookupItem>> GetTrainersByCategoryIdAsync(int categoryId, CancellationToken ct = default);
    Task<SessionDeleteDtoQS?> GetSessionForDeleteOrActivateQSAsync(int id, bool isDelete = true, CancellationToken ct = default);
}
