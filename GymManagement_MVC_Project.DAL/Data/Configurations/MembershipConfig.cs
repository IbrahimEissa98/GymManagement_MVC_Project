using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class MembershipConfig : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable(t =>
        {
            //t.HasCheckConstraint("CK_Membership_StartDate", "StartDate >= GetDate()");
            t.HasCheckConstraint("CK_Membership_EndDate", "EndDate > StartDate");
        });

    }
}
