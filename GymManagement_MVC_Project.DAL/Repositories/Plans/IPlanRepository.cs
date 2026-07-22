using GymManagement_MVC_Project.DAL.Models;

namespace GymManagement_MVC_Project.DAL.Repositories.Plans;

public interface IPlanRepository
{
    Task<IEnumerable<Plan>> GetAllPlansAsync();
    Task<Plan?> GetPlanByIdAsync(int id);
    Task Add(Plan plan);
    void Update(Plan plan);
    void Remove(Plan plan);
    Task<int> SaveChangesAsync();
}
