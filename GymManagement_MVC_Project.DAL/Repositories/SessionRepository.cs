using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

internal class SessionRepository(GymDbContext gymDbContext) : Repository<Session>(gymDbContext), ISessionRepository
{
    private readonly GymDbContext _gymDbContext = gymDbContext;
}
