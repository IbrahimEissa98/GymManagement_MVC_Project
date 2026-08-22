using GymManagement_MVC_Project.BLL.DTOs.Session.Enums;

namespace GymManagement_MVC_Project.PL.ViewModels.Booking;

public class BookingIndexViewModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = default!;
    public SessionStatus Status { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public string Date { get; set; } = default!;
    public string TimeDisplay { get; set; } = default!;
    public TimeSpan Duration { get; set; }
    public string Capacity { get; set; } = default!;
    public bool IsDeleted { get; set; }
}
