using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class MemberConfig : GymUserConfig<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.HasMany(m => m.Bookings)
                .WithOne(b => b.Member)
                .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(m => m.Memberships)
                .WithOne(ms => ms.Member)
                .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(m => m.HealthRecord)
                .WithOne(hr => hr.Member)
                .OnDelete(DeleteBehavior.ClientCascade);
    }
}
