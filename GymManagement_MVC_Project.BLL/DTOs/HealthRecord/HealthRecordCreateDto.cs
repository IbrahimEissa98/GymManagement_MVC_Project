namespace GymManagement_MVC_Project.BLL.DTOs.HealthRecord;

public class HealthRecordCreateDto
{
    public int Height { get; set; }
    public int Weight { get; set; }
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; } = default!;
}
