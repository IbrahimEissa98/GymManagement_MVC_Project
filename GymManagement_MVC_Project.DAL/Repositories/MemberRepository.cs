using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class MemberRepository(GymDbContext gymDbContext) : 
                            Repository<Member>(gymDbContext), 
                            IMemberRepository
{
    //private readonly GymDbContext _gymDbContext = gymDbContext;
    public async Task<bool> IsEmailExistAsync(string email, CancellationToken ct = default)
        => await ExistsAsync(m => m.Email == email, ct);

    public async Task<bool> IsPhoneExistAsync(string phone, CancellationToken ct = default)
        => await ExistsAsync(m => m.Phone == phone, ct);
}
