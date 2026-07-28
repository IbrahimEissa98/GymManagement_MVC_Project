using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

internal static class CategorySeeder
{
    public static async Task SeedCategoriesAsync(GymDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync())
            return;

        var Categories = new List<Category>
            {
                new()
                {
                    Name = "Yoga",
                },
                new()
                {
                    Name = "Cardio",
                },
                new()
                {
                    Name = "Strength Training",
                },
                new()
                {
                    Name = "Cross Fit",
                },
                new()
                {
                    Name = "Boxing",
                },
                new()
                {
                    Name = "General Fitness",
                }
            };

        await dbContext.Categories.AddRangeAsync(Categories);
        await dbContext.SaveChangesAsync();
    }
}
