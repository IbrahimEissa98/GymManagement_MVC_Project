using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface ITrainerRepository : IRepository<Trainer>
{
    Task<bool> IsEmailTakenAsync(string email, int? includeId = null, CancellationToken ct = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? includeId = null, CancellationToken ct = default);
    Task<bool> IsHasScheduledSessionsAsync(int id, CancellationToken ct = default);
}
