using GymManagementProject.Data.Contexts;

namespace GymManagementProject.Data.Seeder;

public static class DatabaseSeeder
{
    public static async Task SeedAllAsync()
    {
        using var dbContext = new GymDbContext();
        await PlanSeeder.SeedPlansAsync(dbContext);
    }
}
