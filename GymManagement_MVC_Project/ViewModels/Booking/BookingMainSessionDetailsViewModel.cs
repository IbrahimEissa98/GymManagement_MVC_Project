namespace GymManagement_MVC_Project.PL.ViewModels.Booking;

public class BookingMainSessionDetailsViewModel
{
    public int SessionId { get; set; }
    public string CategoryName { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
}
