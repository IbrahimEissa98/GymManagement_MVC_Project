using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Booking;
using GymManagement_MVC_Project.BLL.Extensions.Mapping;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class BookingService(
    IUnitOfWork unitOfWork,
    IBookingQueryService queryService,
    IDateTimeProvider clock
    ) : IBookingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBookingQueryService _queryService = queryService;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<BookingIndexDto>>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessions = await _queryService.GetIndexSessionsAsync(ct);
        return Result<IReadOnlyList<BookingIndexDto>>.Success([.. sessions.Select(s => s.GetIndexDto())]);
    }

    public async Task<Result<BookingViewMembersDto>> GetSessionBookings(int sessionId, CancellationToken ct = default)
    {
        var bookings = await _queryService.GetAllSessionBookings(sessionId, ct);
        //var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId, ct);
        if (bookings is null)
            return Result<BookingViewMembersDto>.Failure("Session not found.", ErrorType.NotFound);
        if (bookings.SessionDetails.EndDate < DateTime.UtcNow)
            return Result<BookingViewMembersDto>.Failure("Session is completed.", ErrorType.Failure);

        var bookingsDto = bookings.GetBookingDto();
        return Result<BookingViewMembersDto>.Success(bookingsDto);
    }

    public async Task<Result<BookingMainSessionDetailsDto>> GetMainSessionDetailsAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _queryService.GetMainSessionDetailsAsync(sessionId, ct);
        if (session is null)
            return Result<BookingMainSessionDetailsDto>.Failure("Session not found.", ErrorType.NotFound);

        return Result<BookingMainSessionDetailsDto>.Success(session.GetMainSessionDetailsDto());
    }

    public async Task<Result<IReadOnlyList<DTOs.Booking.MemberLookupItem>>> GetMembersToCreateAsync(int sessionId, CancellationToken ct = default)
    {
        var members = await _queryService.GetMembersToCreateAsync(sessionId, ct);
        if (members is null)
            return Result<IReadOnlyList<DTOs.Booking.MemberLookupItem>>.Success([]);

        return Result<IReadOnlyList<DTOs.Booking.MemberLookupItem>>.Success(
            [..members.Select(m => new DTOs.Booking.MemberLookupItem
            {
                Id = m.Id,
                Name = m.Name
            })]);
    }

    public async Task<Result<IReadOnlyList<BookingSessionMemberDto>>> GetSessionBookedMembersAsync(int sessionId, CancellationToken ct = default)
    {
        var bookedMembers = await _queryService.GetSessionBookedMembersAsync(sessionId, ct);
        if (bookedMembers is null)
            return Result<IReadOnlyList<BookingSessionMemberDto>>.Success([]);

        return Result<IReadOnlyList<BookingSessionMemberDto>>.Success(
            [.. bookedMembers.Select(m => m.GetBookingMember())]);
    }

    public async Task<Result> CreateAsync(BookingCreateDto createDto, CancellationToken ct = default)
    {
        var session = await _unitOfWork.Sessions.GetByIdWithIncludesAsync(
            createDto.SessionId,
            includes: s => s.Bookings,
            ct: ct);
        if (session is null)
            return Result.Failure("Session not found", ErrorType.NotFound);
        if (session.StartDate < _clock.UtcNow)
            return Result.Failure("Cannot create booking at ongoing or completed sessions.", ErrorType.Failure);
        if (session.Capacity <= session.Bookings.Count)
            return Result.Failure("Cannot create booking at full booked sessions.", ErrorType.Failure);
        if (session.Bookings.Any(b => b.MemberId == createDto.MemberId))
            return Result.Failure("Member cannot book the same session twice.", ErrorType.Validation, nameof(createDto.MemberId));

        var memberships = await _unitOfWork.Memberships.FindAsync(ms => ms.MemberId == createDto.MemberId, ct);
        if (!memberships.Any(ms => ms.EndDate > session.EndDate))
            return Result.Failure("Member has no active membership.", ErrorType.Validation, nameof(createDto.MemberId));

        try
        {
            await _unitOfWork.Bookings.AddAsync(createDto.GetBooking(_clock), ct);
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to create Booking.", ErrorType.Failure);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ErrorType.Failure);
        }
    }

    public async Task<Result> CancelAsync(int bookingId, int sessionId, CancellationToken ct = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdWithIncludesAsync(
            bookingId,
            includes: b => b.Session,
            ct: ct);
        if (booking is null)
            return Result.Failure("Booking not found.", ErrorType.NotFound);
        if (booking.Session is null)
            return Result.Failure("Session not found.", ErrorType.NotFound);
        if (booking.Session.Id != sessionId)
            return Result.Failure("Session not found.", ErrorType.Conflict);
        if (booking.Session.StartDate <= _clock.UtcNow)
            return Result.Failure("Cannot cancel booking for ongoing or completed session.", ErrorType.Conflict);
        try
        {
            _unitOfWork.Bookings.Remove(booking);
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to cancel booking.", ErrorType.Failure);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ErrorType.Failure);
        }
    }

    public async Task<Result> AttendAsync(int bookingId, int sessionId, CancellationToken ct = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdWithIncludesAsync(
            bookingId,
            includes: b => b.Session,
            ct: ct);
        if (booking is null)
            return Result.Failure("Booking not found.", ErrorType.NotFound);
        if (booking.Session is null)
            return Result.Failure("Session not found.", ErrorType.NotFound);
        if (booking.Session.Id != sessionId)
            return Result.Failure("Session not found.", ErrorType.Conflict);
        if (booking.IsAttended)
            return Result.Failure("This member is already attended.", ErrorType.Conflict);
        if (booking.Session.StartDate > _clock.UtcNow)
            return Result.Failure("Cannot mark member attend at upcoming session.", ErrorType.Conflict);
        if (booking.Session.EndDate < _clock.UtcNow)
            return Result.Failure("Cannot mark member attend at completed session.", ErrorType.Conflict);

        try
        {
            booking.IsAttended = true;
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to attend member.", ErrorType.Failure);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ErrorType.Failure);
        }
    }


    //public async Task<Result<BookingCreateDto>> GetDataToCreateAsync(int sessionId, CancellationToken ct = default)
    //{
    //    var session = await _queryService.GetMainSessionDetailsAsync(sessionId, ct);
    //    if (session is null)
    //        return Result<BookingCreateDto>.Failure("Session not found.", ErrorType.NotFound);
    //    if (session.EndDate < DateTime.UtcNow)
    //        return Result<BookingCreateDto>.Failure("Session is completed.", ErrorType.Failure);

    //    var bookedCount = await _queryService.GetAllSessionBookings
    //    if (session.SessionDetails..Bookings.Count >= session.Capacity)
    //        return Result<BookingCreateDto>.Failure("Session is full booked.", ErrorType.Failure);

    //    var createDto = await _queryService.GetMembersToCreate(sessionId, ct);
    //    return Result<BookingCreateDto>.Success(createDto.GetCreateDto());
    //}
}
