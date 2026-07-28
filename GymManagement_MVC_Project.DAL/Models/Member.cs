namespace GymManagement_MVC_Project.DAL.Models;

public class Member : GymUser
{
    public string? Photo { get; set; } = default!;

    public HealthRecord HealthRecord { get; set; } = default!;

    public ICollection<Membership> Memberships { get; set; } = [];

    public ICollection<Booking> Bookings { get; set; } = [];

}
