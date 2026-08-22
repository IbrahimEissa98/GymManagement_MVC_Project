using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class PlanRepository(GymDbContext gymDbContext) : Repository<Plan>(gymDbContext), IPlanRepository
{
    private readonly GymDbContext _dbContext = gymDbContext;


}
