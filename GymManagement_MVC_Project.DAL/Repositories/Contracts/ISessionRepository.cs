using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface ISessionRepository : IRepository<Session>
{
    Task<bool> IsSameSpecialties(int categoryId, int TrainerId, CancellationToken ct = default);
    Task<bool> IsTrainerFree(int TrainerId, DateTime start, DateTime end, int? excludeSessionId = null, CancellationToken ct = default);
}
