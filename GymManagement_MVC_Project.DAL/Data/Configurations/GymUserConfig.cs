using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class GymUserConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : GymUser
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(u => u.Name)
                .HasColumnType("VarChar")
                .HasMaxLength(50);

        builder.Property(u => u.Email)
                .HasColumnType("VarChar")
                .HasMaxLength(100);
        builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("ISDeleted = 0");

        builder.Property(u => u.Phone)
                .HasColumnType("VarChar")
                .HasMaxLength(11);
        builder.HasIndex(u => u.Phone)
                .IsUnique()
                .HasFilter("ISDeleted = 0");

        builder.Property(u => u.Gender)
                .HasColumnType("VarChar")
                .HasConversion<string>()
                .HasMaxLength(6);

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Street)
                    .HasColumnType("VarChar")
                    .HasColumnName("Street")
                    .HasMaxLength(30);
            address.Property(a => a.City)
                    .HasColumnType("VarChar")
                    .HasColumnName("City")
                    .HasMaxLength(30);
            address.Property(a => a.BuildingNumber)
                    .HasColumnType("VarChar")
                    .HasColumnName("BuildingNumber")
                    .HasMaxLength(30);
        });

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CK_Email", "Email Like '_%@__%.__%'");
            tb.HasCheckConstraint("CK_Phone",
                "Phone Like '01[0125]%' And Phone Not Like '%[^0-9]%' And Len(Phone) = 11");
            tb.HasCheckConstraint("CK_DateOfBirth",
                "DateOfBirth <= DateAdd(Year, -2, GetDate()) And DateOfBirth >= DateAdd(Year, -100, GetDate())");
        });

        builder.HasQueryFilter(u => !u.IsDeleted);

    }
}
