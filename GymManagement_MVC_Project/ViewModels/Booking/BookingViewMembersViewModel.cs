namespace GymManagement_MVC_Project.PL.ViewModels.Booking;

public class BookingViewMembersViewModel
{
    public BookingMainSessionDetailsViewModel SessionDetails { get; set; } = default!;

    public IReadOnlyList<BookingSessionMemberViewModel> SessionMembers { get; set; } = [];
}
