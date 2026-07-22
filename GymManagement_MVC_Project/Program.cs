using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Data.Seeder;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using GymManagement_MVC_Project.DAL.Repositories.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddDbContext<GymDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.LogTo(Console.WriteLine, LogLevel.Information);

    options.AddInterceptors(new TimestampInterceptor());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await using var scope = app.Services.CreateAsyncScope();

var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

await DatabaseSeeder.SeedAllAsync(dbContext);

app.Run();
