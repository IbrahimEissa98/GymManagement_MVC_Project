using GymManagement_MVC_Project.BLL.Extensions;
using GymManagement_MVC_Project.BLL.Profiles;
using GymManagement_MVC_Project.DAL;
using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Data.Seeder;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("The Default Connection string not found");

var autoMapperLicenseKey = builder.Configuration.GetSection("AutoMapper:LicenseKey").Value
        ?? throw new InvalidOperationException("The AutoMapper License Key not found");

builder.Services.AddGymDataAccess(connectionString);
builder.Services.AddGymBusinessLogic();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserTimeZoneService, UserTimeZoneService>();


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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await using var scope = app.Services.CreateAsyncScope();
var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
if (app.Environment.IsDevelopment())
{
    await dbContext.Database.MigrateAsync();
    await DatabaseSeeder.SeedAllAsync(dbContext);
}
else
{
    await DatabaseSeeder.SeedAllJsonAsync(dbContext);
}

app.Run();
