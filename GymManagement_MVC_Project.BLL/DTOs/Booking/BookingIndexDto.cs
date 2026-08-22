using GymManagement_MVC_Project.BLL.DTOs.Session.Enums;

namespace GymManagement_MVC_Project.BLL.DTOs.Booking;

public class BookingIndexDto
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TrainerName { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public int BookedCount { get; set; }
    public SessionStatus Status { get; set; } = default!;
    public bool IsDeleted { get; set; }
}
