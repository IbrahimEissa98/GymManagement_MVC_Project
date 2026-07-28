using GymManagement_MVC_Project.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_MVC_Project.DAL.Data.Configurations;

internal class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
                .HasColumnType("VarChar")
                .HasMaxLength(20);
    }
}
