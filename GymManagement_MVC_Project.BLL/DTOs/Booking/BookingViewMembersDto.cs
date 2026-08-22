namespace GymManagement_MVC_Project.BLL.DTOs.Booking;

public class BookingViewMembersDto
{
    public BookingMainSessionDetailsDto SessionDetails { get; set; } = default!;

    public IReadOnlyList<BookingSessionMemberDto> SessionMembers { get; set; } = [];
}
