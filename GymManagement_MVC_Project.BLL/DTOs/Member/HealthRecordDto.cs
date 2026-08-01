namespace GymManagement_MVC_Project.BLL.DTOs.Member;

public class HealthRecordDto
{
    public int Height { get; set; }
    public int Weight { get; set; }
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; } = default!;
}
