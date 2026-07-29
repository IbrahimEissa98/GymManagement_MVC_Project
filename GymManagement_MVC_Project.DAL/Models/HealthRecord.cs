using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.DAL.Models;

public class HealthRecord : BaseEntity
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public BloodTypes BloodType { get; set; }
    public string? Note { get; set; } = default!;

    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
}
