using GymManagement_MVC_Project.DAL.Models.Interceptors;

namespace GymManagement_MVC_Project.DAL.Models;

public class Plan : BaseEntity, IHasTimestamps
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
