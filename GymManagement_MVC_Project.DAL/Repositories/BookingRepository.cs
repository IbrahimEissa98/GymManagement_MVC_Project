using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

internal class BookingRepository(GymDbContext gymDbContext) : Repository<Booking>(gymDbContext), IBookingRepository
{
}
