using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Data.Seeder.Models;
using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

internal class PlanSeeder
{
    internal static async Task SeedPlansAsync(GymDbContext dbContext)
    {
        if (await dbContext.Plans.AnyAsync())
            return;

        var plans = new List<Plan>
        {
            new()
            {
                Name = "Basic Plan",
                Description = "Basic banking plan with essential services.",
                DurationDays = 30,
                Price = 300m,
                IsActive = true
            },
            new()
            {
                Name = "Standard Plan",
                Description = "Includes higher transaction limits and additional benefits.",
                DurationDays = 60,
                Price = 500m,
                IsActive = true
            },
            new()
            {
                Name = "Premium Plan",
                Description = "Premium banking plan with priority support and exclusive features.",
                DurationDays = 90,
                Price = 900m,
                IsActive = true
            },
            new()
            {
                Name = "Annual Plan",
                Description = "Ultimate banking experience with all premium services included.",
                DurationDays = 365,
                Price = 3000m,
                IsActive = true
            }
        };

        await dbContext.Plans.AddRangeAsync(plans);
        await dbContext.SaveChangesAsync();
    }

    internal static async Task SeedPlansFromJsonAsync(GymDbContext dbContext)
    {
        //if (await dbContext.Plans.AnyAsync())
        //    return;

        var seederPlans = await SeedJsonLoader.LoadAsync<PlanSeedModel>("plans.json");
        if (seederPlans is null || !seederPlans.Any())
            return;

        var plans = seederPlans.Select(p => new Plan
        {
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            DurationDays = p.DurationDays,
            IsActive = p.IsActive
        });

        await dbContext.Plans.AddRangeAsync(plans);
        await dbContext.SaveChangesAsync();
    }
}