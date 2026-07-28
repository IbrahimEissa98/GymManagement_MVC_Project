using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class HealthRecordConfig : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.Property(hr => hr.Weight)
                .HasPrecision(5, 2);

        builder.Property(hr => hr.Height)
                .HasPrecision(5, 2);

        builder.Property(hr => hr.BloodType)
                .HasConversion<string>()
                .HasMaxLength(20);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_HealthRecord_Height", "Height > 0");
            t.HasCheckConstraint("CK_HealthRecord_Weight", "Weight > 0");
        });


    }
}
