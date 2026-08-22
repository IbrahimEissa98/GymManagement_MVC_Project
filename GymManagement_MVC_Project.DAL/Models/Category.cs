using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.DAL.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public TrainerSpecialties? Specialties { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];
}
