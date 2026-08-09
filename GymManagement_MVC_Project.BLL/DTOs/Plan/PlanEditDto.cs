namespace GymManagement_MVC_Project.BLL.DTOs.Plan;

public class PlanEditDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    //public bool IsActive { get; set; }
}
