using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface IMemberRepository : IRepository<Member>
{
    Task<bool> IsEmailTakenAsync(string email, int? includeId = null, CancellationToken ct = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? includeId = null, CancellationToken ct = default);
    Task<Member?> GetByIdWithMembershipAsync(int id, DateTime date, CancellationToken ct = default);
    Task<bool> IsHasUpcomingBookingAsync(int id, DateTime date, CancellationToken ct = default);
}
