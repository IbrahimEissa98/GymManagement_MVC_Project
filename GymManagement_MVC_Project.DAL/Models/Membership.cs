namespace GymManagement_MVC_Project.DAL.Models;

public class Membership : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = default!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
