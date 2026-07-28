using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class SessionConfig : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(s => s.Description)
                .HasColumnType("VarChar")
                .HasMaxLength(300);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Session_Capacity", "Capacity between 1 and 25");
            t.HasCheckConstraint("CK_Session_StartDate", "StartDate > GetDate()");
            t.HasCheckConstraint("CK_Session_EndDate", "EndDate > StartDate");
        });
    }
}
