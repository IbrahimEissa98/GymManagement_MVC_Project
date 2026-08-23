using GymManagement_MVC_Project.BLL.Extensions;
using GymManagement_MVC_Project.BLL.Profiles;
using GymManagement_MVC_Project.DAL;
using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Data.Seeder;
using GymManagement_MVC_Project.DAL.Identity;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(op =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    op.Filters.Add(new AuthorizeFilter(policy));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("The Default Connection string not found");

var autoMapperLicenseKey = builder.Configuration.GetSection("AutoMapper:LicenseKey").Value
        ?? throw new InvalidOperationException("The AutoMapper License Key not found");

builder.Services.AddGymDataAccess(connectionString);
builder.Services.AddGymBusinessLogic();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserTimeZoneService, UserTimeZoneService>();

builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>(op =>
                {
                    op.Password.RequiredLength = 8;
                    op.Password.RequireDigit = true;

                    op.User.RequireUniqueEmail = true;

                    op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    op.Lockout.MaxFailedAccessAttempts = 3;

                    op.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<GymDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.AccessDeniedPath = "";
    config.ExpireTimeSpan = TimeSpan.FromHours(24);
    config.SlidingExpiration = true;
});

builder.Services.AddAutoMapper(config =>
    {
        config.LicenseKey = autoMapperLicenseKey;
    },
    typeof(PlanDtoProfile).Assembly, typeof(PlanVMProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseRouting();

app.UseAuthorization();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await using var scope = app.Services.CreateAsyncScope();
var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
if (app.Environment.IsDevelopment())
{
    await dbContext.Database.MigrateAsync();
    await DatabaseSeeder.SeedAllJsonAsync(dbContext, userManager, roleManager, app.Configuration);
}
else
{
    await DatabaseSeeder.SeedAllJsonAsync(dbContext, userManager, roleManager, app.Configuration);
}

app.Run();
