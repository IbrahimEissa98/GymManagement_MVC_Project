using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Repositories.Plans;

public class PlanRepository : IPlanRepository
{
    private readonly GymDbContext _dbContext;

    public PlanRepository(GymDbContext gymDbContext)
    {
        _dbContext = gymDbContext;
    }

    public async Task<IEnumerable<Plan>> GetAllPlansAsync()
        => await _dbContext.Plans.ToListAsync();

    public async Task<Plan?> GetPlanByIdAsync(int id)
        => await _dbContext.Plans.FindAsync(id);

    public async Task Add(Plan plan)
        => await _dbContext.Plans.AddAsync(plan);

    public void Update(Plan plan)
        => _dbContext.Plans.Update(plan);

    public void Remove(Plan plan)
        => _dbContext.Plans.Remove(plan);

    public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
}
