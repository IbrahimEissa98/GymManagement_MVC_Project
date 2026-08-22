namespace GymManagement_MVC_Project.BLL.DTOs.Member;

public class MemberDetailsDto
{
    public int Id { get; set; }
    public string? PhotoUrl { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string? PlanName { get; set; } = default!;
    public string? MembershipStartDate { get; set; } = default!;
    public string? MembershipEndDate { get; set; } = default!;
    public string Address { get; set; } = default!;
}
