using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Queries.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Queries;

public class BookingQueryService(GymDbContext gymDbContext) : IBookingQueryService
{
    private readonly GymDbContext _gymDbContext = gymDbContext;

    public async Task<IReadOnlyList<BookingIndexDtoQS>> GetIndexSessionsAsync(CancellationToken ct = default)
    {
        return [..await _gymDbContext.Sessions
            .AsNoTracking()
            .Where(s => s.EndDate >= DateTime.UtcNow)
            .Select(s => new BookingIndexDtoQS
            {
                Id = s.Id,
                Capacity = s.Capacity,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                CategoryName = s.Category.Name,
                BookedCount = s.Bookings.Count,
                TrainerName = s.Trainer.Name,
                IsDeleted = s.IsDeleted
            }).ToListAsync(ct)];
    }

    public async Task<BookingViewMembersDtoQS?> GetAllSessionBookings(int sessionId, CancellationToken ct = default)
    {
        var sessionDetails = await GetMainSessionDetailsAsync(sessionId, ct);
        if (sessionDetails is null)
            return null;

        var bookedMembers = await GetSessionBookedMembersAsync(sessionId, ct);

        return new BookingViewMembersDtoQS
        {
            SessionDetails = sessionDetails,
            SessionMembers = bookedMembers
        };
    }

    public async Task<BookingCreateDtoQS?> GetDataToCreate(int sessionId, CancellationToken ct = default)
    {
        var sessionDetails = await GetMainSessionDetailsAsync(sessionId, ct);
        if (sessionDetails is null)
            return null;

        var members = await GetMembersToCreateAsync(sessionId, ct);
        return new BookingCreateDtoQS
        {
            SessionDetails = sessionDetails,
            Members = members
        };
    }

    public async Task<BookingMainSessionDetailsDtoQS?> GetMainSessionDetailsAsync(int sessionId, CancellationToken ct = default)
    {
        return await _gymDbContext.Sessions
                            .Where(s => s.Id == sessionId)
                            .Select(s => new BookingMainSessionDetailsDtoQS
                            {
                                SessionId = s.Id,
                                TrainerName = s.Trainer.Name,
                                CategoryName = s.Category.Name,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                                Capacity = s.Capacity
                            }).FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<MemberLookupItem>> GetMembersToCreateAsync(int sessionId, CancellationToken ct = default)
    {
        return [..await _gymDbContext.Members
                                                        .Where(m => m.Memberships
                                                            .Any(ms => ms.EndDate > DateTime.UtcNow
                                                                    && ms.IsDeleted == false)
                                                            && (m.Bookings.Count == 0 || !m.Bookings
                                                            .Any(b => b.SessionId == sessionId)))
                                                        .Select(m => new MemberLookupItem
                                                        {
                                                            Id = m.Id,
                                                            Name = m.Name
                                                        }).ToListAsync(ct)];
    }

    public async Task<IReadOnlyList<BookingSessionMemberDtoQS>> GetSessionBookedMembersAsync(int sessionId, CancellationToken ct = default)
    {
        return [..await _gymDbContext.Bookings
                    .AsNoTracking()
                    .Where(s => s.SessionId == sessionId)
                    .Select(b => new BookingSessionMemberDtoQS
                    {
                        Id = b.Id,
                        MemberName = b.Member.Name,
                        BookingDate = b.BookingDate,
                        IsAttended = b.IsAttended
                    }).ToListAsync(ct)];
    }
}
