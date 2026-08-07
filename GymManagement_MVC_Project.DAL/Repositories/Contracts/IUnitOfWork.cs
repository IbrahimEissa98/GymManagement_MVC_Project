using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface IUnitOfWork
{
    IPlanRepository Plans { get; }
    IMemberRepository Members { get; }
    IRepository<HealthRecord> HealthRecords { get; }
    ITrainerRepository Trainers { get; }
    ISessionRepository Sessions { get; }
    IRepository<Category> Categories { get; }
    IMembershipRepository Memberships { get; }
    IBookingRepository Bookings { get; }

    Task<int> CommitAsync(CancellationToken ct = default);
}
