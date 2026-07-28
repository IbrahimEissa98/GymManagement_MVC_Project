using GymManagement_MVC_Project.DAL.Data.Contexts;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

public static class DatabaseSeeder
{
    public static async Task SeedAllAsync(GymDbContext dbContext)
    {
        await PlanSeeder.SeedPlansAsync(dbContext);
        await CategorySeeder.SeedCategoriesAsync(dbContext);
    }
}
