namespace GymManagement_MVC_Project.PL.ViewModels.Membership;

public class MembershipIndexViewModel
{
    public int Id { get; set; }
    public string MemberName { get; set; } = default!;
    public string PlanName { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime EndDate { get; set; } = default!;
}
