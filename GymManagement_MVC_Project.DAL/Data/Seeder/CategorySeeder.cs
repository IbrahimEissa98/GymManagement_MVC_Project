using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
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
                    Specialties = TrainerSpecialties.Yoga
                },
                new()
                {
                    Name = "Cardio",
                    Specialties = TrainerSpecialties.Cardio
                },
                new()
                {
                    Name = "Strength Training",
                    Specialties = TrainerSpecialties.StrengthTrainer
                },
                new()
                {
                    Name = "Cross Fit",
                    Specialties = TrainerSpecialties.CrossFit
                },
                new()
                {
                    Name = "Boxing",
                    Specialties = TrainerSpecialties.Boxing
                },
                new()
                {
                    Name = "General Fitness",
                    Specialties = TrainerSpecialties.GeneralFitness
                }
            };

        await dbContext.Categories.AddRangeAsync(Categories);
        await dbContext.SaveChangesAsync();
    }
}
