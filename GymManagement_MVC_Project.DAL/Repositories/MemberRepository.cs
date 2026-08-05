using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class MemberRepository(GymDbContext gymDbContext) :
                            Repository<Member>(gymDbContext),
                            IMemberRepository
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    //private readonly GymDbContext _gymDbContext = gymDbContext;
    public async Task<bool> IsEmailExistAsync(string email, int? includeId = null, CancellationToken ct = default)
        => await ExistsAsync(m => m.Email == email && 
                            (!includeId.HasValue || m.Id != includeId.Value),
                            ct);

    public async Task<bool> IsPhoneExistAsync(string phone, int? includeId = null, CancellationToken ct = default)
        => await ExistsAsync(m => m.Phone == phone &&
                            (!includeId.HasValue || m.Id != includeId.Value), ct);

    public async Task<Member?> GetByIdWithMembershipAsync(int id, CancellationToken ct = default)
        => await _gymDbContext.Members.AsNoTracking()
                                    .Include(m => m.Memberships)
                                        .ThenInclude(ms => ms.Plan)
                                    .FirstOrDefaultAsync(m => m.Id == id, ct);
    public async Task<bool> IsHasUpcomingBookingAsync(int id, CancellationToken ct = default)
        => await _gymDbContext.Members.AsNoTracking()
                                    .Where(m => m.Id == id)
                                    .SelectMany(m => m.Bookings)
                                    .AnyAsync(b => b.Session.EndDate > DateTime.UtcNow, ct);

}
