using GymManagement_MVC_Project.DAL.Identity;
using GymManagement_MVC_Project.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagement_MVC_Project.DAL.Data.Contexts;

public class GymDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Not(property), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppIdentityUser>()
                    .Property(u => u.FullName)
                    .HasMaxLength(150);
        modelBuilder.Entity<AppIdentityRole>()
                    .Property(u => u.DisplayName)
                    .HasMaxLength(50);
    }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Membership> Memberships { get; set; }

}
