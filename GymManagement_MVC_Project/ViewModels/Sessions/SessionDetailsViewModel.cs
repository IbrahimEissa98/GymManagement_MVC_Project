using GymManagement_MVC_Project.BLL.DTOs.Session.Enums;

namespace GymManagement_MVC_Project.PL.ViewModels.Sessions;

public class SessionDetailsViewModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = default!;
    public SessionStatus Status { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public string StartDate { get; set; } = default!;
    public string EndDate { get; set; } = default!;
    public TimeSpan Duration { get; set; }
    public int Capacity { get; set; } = default!;
    public int BookedCount { get; set; } = default!;
}
