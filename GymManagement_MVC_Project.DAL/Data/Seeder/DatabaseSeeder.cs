using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Data.Seeder.Identity;
using GymManagement_MVC_Project.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

public static class DatabaseSeeder
{
    public static async Task SeedAllAsync(
        GymDbContext dbContext,
        UserManager<AppIdentityUser> userManager,
        RoleManager<AppIdentityRole> roleManager,
        IConfiguration config)
    {
        await PlanSeeder.SeedPlansAsync(dbContext);
        await CategorySeeder.SeedCategoriesAsync(dbContext);
        await IdentitySeeder.SeedAsync(config, userManager, roleManager);
    }

    public static async Task SeedAllJsonAsync(
        GymDbContext dbContext,
        UserManager<AppIdentityUser> userManager,
        RoleManager<AppIdentityRole> roleManager,
        IConfiguration config)
    {
        await PlanSeeder.SeedPlansFromJsonAsync(dbContext);
        await CategorySeeder.SeedCategoriesAsync(dbContext);
        await IdentitySeeder.SeedAsync(config, userManager, roleManager);
    }
}
