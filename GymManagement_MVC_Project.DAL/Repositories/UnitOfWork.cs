using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class UnitOfWork(GymDbContext gymDbContext) : IUnitOfWork
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    private IPlanRepository? _plans;
    private IMemberRepository? _members;
    private IRepository<HealthRecord>? _healthRecords;
    private ITrainerRepository? _trainers;
    private ISessionRepository? _sessions;
    private IRepository<Category>? _categories;
    private IMembershipRepository? _memberships;
    private IBookingRepository? _bookings;

    public IPlanRepository Plans => _plans ??= new PlanRepository(_gymDbContext);

    public IMemberRepository Members => _members ??= new MemberRepository(_gymDbContext);

    public IRepository<HealthRecord> HealthRecords => _healthRecords ??= new Repository<HealthRecord>(_gymDbContext);

    public ITrainerRepository Trainers => _trainers ??= new TrainerRepository(_gymDbContext);

    public ISessionRepository Sessions => _sessions ??= new SessionRepository(_gymDbContext);

    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_gymDbContext);

    public IMembershipRepository Memberships => _memberships ??= new MembershipRepository(_gymDbContext);

    public IBookingRepository Bookings => _bookings ??= new BookingRepository(_gymDbContext);


    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await _gymDbContext.SaveChangesAsync(ct);
    }
}
