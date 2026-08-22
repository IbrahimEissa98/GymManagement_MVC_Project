namespace GymManagement_MVC_Project.BLL.DTOs.Booking;

public class BookingCreateDto
{
    public int MemberId { get; set; }
    public int SessionId { get; set; }
}
//public class BookingCreateDto
//{
//    public BookingMainSessionDetailsDto SessionDetails { get; set; } = default!;

//    public IReadOnlyList<MemberLookupItem> Members { get; set; } = [];
//}
