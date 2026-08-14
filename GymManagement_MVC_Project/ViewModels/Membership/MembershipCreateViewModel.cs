using GymManagement_MVC_Project.BLL.DTOs.Membership.Lookup;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Membership;

public class MembershipCreateViewModel
{
    [Required(ErrorMessage = "Member is required.")]
    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Plan is required.")]
    [Range(1, 10)]
    public int PlanId { get; set; }

    public IReadOnlyList<MemberLookupItem> Members { get; set; } = [];
    public IReadOnlyList<PlanLookupItem> Plans { get; set; } = [];
}
