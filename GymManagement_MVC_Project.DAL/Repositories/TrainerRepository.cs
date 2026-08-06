using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class TrainerRepository(GymDbContext gymDbContext) : Repository<Trainer>(gymDbContext), ITrainerRepository
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    public async Task<bool> IsEmailTakenAsync(string email, int? includeId = null, CancellationToken ct = default)
    {
        return await ExistsAsync(t => t.Email == email && (!includeId.HasValue || t.Id != includeId), ct);
    }

    public async Task<bool> IsPhoneTakenAsync(string phone, int? includeId = null, CancellationToken ct = default)
    {
        return await ExistsAsync(t => t.Phone == phone && (!includeId.HasValue || t.Id != includeId), ct);
    }

    public async Task<bool> IsHasScheduledSessionsAsync(int id, CancellationToken ct = default)
    {
        return await _gymDbContext.Trainers
                                    .AsNoTracking()
                                    .Where(t => t.Id == id)
                                    .SelectMany(t => t.Sessions)
                                    .AnyAsync(b => b.EndDate > DateTime.UtcNow, ct);
    }
}
