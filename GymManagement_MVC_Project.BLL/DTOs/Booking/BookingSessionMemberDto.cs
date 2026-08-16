namespace GymManagement_MVC_Project.BLL.DTOs.Booking;

public class BookingSessionMemberDto
{
    public int Id { get; set; }
    public string MemberName { get; set; } = default!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; }
}
