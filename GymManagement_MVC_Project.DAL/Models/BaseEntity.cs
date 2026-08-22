using GymManagement_MVC_Project.DAL.Models.Interceptors;

namespace GymManagement_MVC_Project.DAL.Models;

public class BaseEntity : IHasTimestamps
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
