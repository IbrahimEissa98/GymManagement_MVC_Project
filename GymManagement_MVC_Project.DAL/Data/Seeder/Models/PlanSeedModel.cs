namespace GymManagement_MVC_Project.DAL.Data.Seeder.Models;

public class PlanSeedModel
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
