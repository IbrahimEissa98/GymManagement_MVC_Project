using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class MemberConfig : GymUserConfig<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.CreatedAt)
                .HasColumnName("JoinDate");
    }
}
