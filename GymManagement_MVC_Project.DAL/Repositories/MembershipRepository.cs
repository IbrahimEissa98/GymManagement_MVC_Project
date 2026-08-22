using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

internal class MembershipRepository(GymDbContext gymDbContext) : Repository<Membership>(gymDbContext), IMembershipRepository
{
}
