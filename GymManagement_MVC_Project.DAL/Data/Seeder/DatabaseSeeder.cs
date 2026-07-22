using GymManagement_MVC_Project.DAL.Data.Contexts;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

public static class DatabaseSeeder
{
    public static async Task SeedAllAsync()
    {
        using var dbContext = new GymDbContext();
        await PlanSeeder.SeedPlansAsync(dbContext);
    }
}
