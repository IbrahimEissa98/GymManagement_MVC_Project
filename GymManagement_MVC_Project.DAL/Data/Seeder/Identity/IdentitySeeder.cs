using GymManagement_MVC_Project.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymManagement_MVC_Project.DAL.Data.Seeder.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IConfiguration config, UserManager<AppIdentityUser> userManager,
                                    RoleManager<AppIdentityRole> roleManager)
    {
        await EnsureRoleExists(roleManager, IdentityRolesName.SuperAdmin, "Super Admin");
        await EnsureRoleExists(roleManager, IdentityRolesName.Admin, "Admin");

        var superAdminEmail = config["IdentitySeeder:SuperAdmin:SuperAdminEmail"] ?? throw new InvalidOperationException();
        var superAdminPass = config["IdentitySeeder:SuperAdmin:SuperAdminPassword"] ?? throw new InvalidOperationException();
        var adminEmail = config["IdentitySeeder:Admin:AdminEmail"] ?? throw new InvalidOperationException();
        var adminPass = config["IdentitySeeder:Admin:AdminPassword"] ?? throw new InvalidOperationException();

        var superAdmin = await EnsureUserExists(userManager, superAdminEmail, superAdminPass, "Super Administrator");
        var admin = await EnsureUserExists(userManager, adminEmail, adminPass, "Administrator");

        await EnsureUserAssignedToRole(userManager, superAdmin!, IdentityRolesName.SuperAdmin);
        await EnsureUserAssignedToRole(userManager, admin!, IdentityRolesName.Admin);
    }


    private static async Task EnsureUserAssignedToRole(UserManager<AppIdentityUser> userManager, AppIdentityUser user, string role)
    {
        if (!await userManager.IsInRoleAsync(user, role))
        {
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to assign role to user.");
        }
    }

    private static async Task<AppIdentityUser?> EnsureUserExists(UserManager<AppIdentityUser> userManager, string email, string pass, string fullName)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new AppIdentityUser
            {
                Email = email,
                UserName = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, pass);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user.");
        }

        return user;
    }

    private static async Task EnsureRoleExists(RoleManager<AppIdentityRole> roleManager, string roleName, string roleDisplayName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result = await roleManager.CreateAsync(new AppIdentityRole
            {
                Name = roleName,
                DisplayName = roleDisplayName
            });
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create role.");
        }
    }
}
