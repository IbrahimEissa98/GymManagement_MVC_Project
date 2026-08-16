using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Booking;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IBookingService
{
    Task<Result<IReadOnlyList<BookingIndexDto>>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<Result<BookingViewMembersDto>> GetSessionBookings(int sessionId, CancellationToken ct = default);
    //Task<Result<BookingCreateDto>> GetDataToCreateAsync(int sessionId, CancellationToken ct = default);
    Task<Result<BookingMainSessionDetailsDto>> GetMainSessionDetailsAsync(int sessionId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DTOs.Booking.MemberLookupItem>>> GetMembersToCreateAsync(int sessionId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<BookingSessionMemberDto>>> GetSessionBookedMembersAsync(int sessionId, CancellationToken ct = default);
    Task<Result> CreateAsync(BookingCreateDto createDto, CancellationToken ct = default);
    Task<Result> CancelAsync(int bookingId, int sessionId, CancellationToken ct = default);
    Task<Result> AttendAsync(int bookingId, int sessionId, CancellationToken ct = default);
}
