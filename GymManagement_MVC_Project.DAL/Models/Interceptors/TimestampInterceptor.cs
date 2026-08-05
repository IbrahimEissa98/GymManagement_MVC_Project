using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GymManagement_MVC_Project.DAL.Models.Interceptors;

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext? context)
    {
        if (context is null) return;

        var now = DateTime.UtcNow;

        var entries = context.ChangeTracker.Entries<IHasTimestamps>();

        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.Metadata.ClrType.Name} | State = {entry.State}");

            foreach (var property in entry.Properties)
            {
                Console.WriteLine($"{property.Metadata.Name} | Modified = {property.IsModified} | Current = {property.CurrentValue}");
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(nameof(IHasTimestamps.CreatedAt)).IsModified = false;
                    break;

                case EntityState.Deleted:
                    //entry.State = EntityState.Modified;

                    //entry.Entity.IsDeleted = true;
                    //entry.Entity.UpdatedAt = now;
                    //entry.Entity.DeletedAt = now;

                    entry.State = EntityState.Unchanged;

                    foreach (var reference in entry.References)
                    {
                        if (reference.TargetEntry?.Metadata.IsOwned() == true)
                        {
                            reference.TargetEntry.State = EntityState.Unchanged;
                        }
                    }

                    entry.Property(e => e.IsDeleted).CurrentValue = true;
                    entry.Property(e => e.IsDeleted).IsModified = true;

                    entry.Property(e => e.DeletedAt).CurrentValue = now;
                    entry.Property(e => e.DeletedAt).IsModified = true;

                    entry.Property(e => e.UpdatedAt).CurrentValue = now;
                    entry.Property(e => e.UpdatedAt).IsModified = true;

                    entry.Property(nameof(IHasTimestamps.CreatedAt)).IsModified = false;
                    break;
            }
        }
    }
}
