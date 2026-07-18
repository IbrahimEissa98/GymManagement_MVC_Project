using GymManagementProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementProject.Data.Configurations;

public class PlanConfig : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

        builder.Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(200);

        builder.Property(p => p.Price)
                .HasPrecision(10, 2);

        //builder.Property(p => p.CreatedAt)
        //        .HasDefaultValueSql("GETDATE()");

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("Ck_PlanDuration", "DurationDays Between 1 And 365");
        });

        builder.HasIndex(p => p.Name)
                .IsUnique();
    }
}
