using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.DAL.Queries.Contracts;

public interface IBookingQueryService
{
    Task<IReadOnlyList<BookingIndexDtoQS>> GetIndexSessionsAsync(CancellationToken ct = default!);
    Task<BookingViewMembersDtoQS?> GetAllSessionBookings(int sessionId, CancellationToken ct = default);
    Task<BookingCreateDtoQS?> GetDataToCreate(int sessionId, CancellationToken ct = default!);
    Task<BookingMainSessionDetailsDtoQS?> GetMainSessionDetailsAsync(int sessionId, CancellationToken ct = default);
    Task<IReadOnlyList<MemberLookupItem>> GetMembersToCreateAsync(int sessionId, CancellationToken ct = default);
    Task<IReadOnlyList<BookingSessionMemberDtoQS>> GetSessionBookedMembersAsync(int sessionId, CancellationToken ct = default);
}
