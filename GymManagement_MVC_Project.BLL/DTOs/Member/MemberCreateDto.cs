using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;

namespace GymManagement_MVC_Project.BLL.DTOs.Member;

public class MemberCreateDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = default!;
    public string BuildingNumber { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public HealthRecordCreateDto HealthRecord { get; set; } = default!;
}