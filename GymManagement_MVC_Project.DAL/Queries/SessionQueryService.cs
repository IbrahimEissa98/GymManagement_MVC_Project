using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Queries.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Queries;

public class SessionQueryService(GymDbContext gymDbContext) : ISessionQueryService
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    public async Task<IReadOnlyList<SessionIndexDtoQS>> GetAllSessionsIndexQSAsync(CancellationToken ct = default)
    {
        return await _gymDbContext.Sessions
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Select(s => new SessionIndexDtoQS
            {
                Id = s.Id,
                Capacity = s.Capacity,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                CategoryName = s.Category.Name,
                BookedCount = s.Bookings.Count,
                TrainerName = s.Trainer.Name,
                IsDeleted = s.IsDeleted
            }).ToListAsync(ct);
    }

    public async Task<SessionIndexDtoQS?> GetSessionDetailsQSAsync(int id, CancellationToken ct = default)
    {
        return await _gymDbContext.Sessions
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SessionIndexDtoQS
            {
                Id = s.Id,
                Capacity = s.Capacity,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                CategoryName = s.Category.Name,
                BookedCount = s.Bookings.Count,
                TrainerName = s.Trainer.Name,
            }).FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TrainerLookupItem>> GetTrainersByCategoryIdAsync(int categoryId, CancellationToken ct = default)
    {
        var category = await _gymDbContext.Categories
                                .FirstOrDefaultAsync(c => c.Id == categoryId, ct);

        if (category is null)
            return [];

        return [..await _gymDbContext.Trainers
                        .AsNoTracking()
                        .Where(t => t.Specialties == category.Specialties)
                        .Select(t => new TrainerLookupItem
                        {
                            Id = t.Id,
                            Name = t.Name
                        })
                        .ToListAsync(ct)];
    }

    public async Task<SessionDeleteDtoQS?> GetSessionForDeleteOrActivateQSAsync(int id, bool isDelete = true, CancellationToken ct = default)
    {
        var query = _gymDbContext.Sessions.Where(s => s.Id == id &&
                                                                (isDelete || s.IsDeleted == true));

        if (!isDelete)
            query = query.IgnoreQueryFilters();

        return await query.Select(s => new SessionDeleteDtoQS
        {
            CategoryName = s.Category.Name,
            TrainerName = s.Trainer.Name
        }).FirstOrDefaultAsync(ct);
    }
}
