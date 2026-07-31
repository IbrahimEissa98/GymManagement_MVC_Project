using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.DAL.Models;

public class Trainer : GymUser
{
    public TrainerSpecialties Specialties { get; set; }
    public DateOnly HireDate { get; set; }

    public ICollection<Session> Sessions { get; set; } = []; 
}
