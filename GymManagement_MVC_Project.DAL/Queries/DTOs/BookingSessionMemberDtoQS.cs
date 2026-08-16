namespace GymManagement_MVC_Project.DAL.Queries.DTOs;

public class BookingSessionMemberDtoQS
{
    public int Id { get; set; }
    public string MemberName { get; set; } = default!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; }
}
