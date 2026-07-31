using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
    private readonly GymDbContext _dbContext;

    public PlanRepository(GymDbContext gymDbContext) : base(gymDbContext)
    {
        _dbContext = gymDbContext;
    }


}
