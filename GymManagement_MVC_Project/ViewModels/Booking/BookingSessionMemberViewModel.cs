namespace GymManagement_MVC_Project.PL.ViewModels.Booking;

public class BookingSessionMemberViewModel
{
    public int Id { get; set; }
    public string MemberName { get; set; } = default!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; }
}
