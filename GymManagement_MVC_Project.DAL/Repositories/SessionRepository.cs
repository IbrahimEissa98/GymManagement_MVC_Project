using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Repositories;

internal class SessionRepository(GymDbContext gymDbContext) : Repository<Session>(gymDbContext), ISessionRepository
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    public async Task<bool> IsSameSpecialties(int categoryId, int trainerId, CancellationToken ct = default)
    {
        var category = await _gymDbContext.Categories
                                .FirstOrDefaultAsync(c => c.Id == categoryId, ct);
        var trainer = await _gymDbContext.Trainers
                                .FirstOrDefaultAsync(t => t.Id == trainerId, ct);
        if (category?.Specialties != trainer?.Specialties)
            return false;

        return true;
    }

    public async Task<bool> IsTrainerFree(int trainerId, DateTime start, DateTime end, int? excludeSessionId = null, CancellationToken ct = default)
    {
        var query = _gymDbContext.Sessions
                                    .AsNoTracking()
                                    .Where(
                                    s => s.TrainerId == trainerId
                                            && s.StartDate < end
                                            && s.EndDate > start);
        if (excludeSessionId.HasValue)
            query = query.Where(s => s.Id != excludeSessionId.Value);

        return !(await query.AnyAsync(ct));
    }
}
