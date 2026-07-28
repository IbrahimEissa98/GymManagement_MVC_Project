using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class BookingConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(b => b.IsAttended)
                .HasDefaultValue(false);

        builder.HasIndex(x => new
        {
            x.SessionId,
            x.MemberId
        }).IsUnique();

    }
}
