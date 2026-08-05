using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface IMemberRepository : IRepository<Member>
{
    Task<bool> IsEmailExistAsync(string email, int? includeId = null, CancellationToken ct = default);
    Task<bool> IsPhoneExistAsync(string phone, int? includeId = null, CancellationToken ct = default);
    Task<Member?> GetByIdWithMembershipAsync(int id, CancellationToken ct = default);
    Task<bool> IsHasUpcomingBookingAsync(int id, CancellationToken ct = default);
}
