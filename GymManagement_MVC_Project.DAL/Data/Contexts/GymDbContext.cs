using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement_MVC_Project.DAL.Data.Contexts;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
    }

    public DbSet<Plan> Plans { get; set; }
}
