using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class TrainerConfig : GymUserConfig<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        //builder.Property(u => u.CreatedAt)
        //        .HasColumnName("HireDate");

        builder.Property(t => t.Specialties)
                .HasColumnType("VarChar")
                .HasConversion<string>()
                .HasMaxLength(20);
    }
}
