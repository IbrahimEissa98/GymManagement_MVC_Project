namespace GymManagement_MVC_Project.DAL.Queries.DTOs;

public class BookingViewMembersDtoQS
{
    public BookingMainSessionDetailsDtoQS SessionDetails { get; set; } = default!;

    public IReadOnlyList<BookingSessionMemberDtoQS> SessionMembers { get; set; } = [];
}
