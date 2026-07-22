using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement_MVC_Project.DAL.Data.Contexts;

public class GymDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = .; Database = GymProject_MVC_Db; Trusted_Connection = True; TrustServerCertificate = True;");

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.AddInterceptors(new TimestampInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
    }

    public DbSet<Plan> Plans { get; set; }
}
