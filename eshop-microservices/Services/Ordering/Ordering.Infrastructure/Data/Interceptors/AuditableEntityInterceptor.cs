using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor: SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    
    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = "mehmet";
            }

            if (entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = "mehmet";
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) => entry.References.Any(reference => reference.TargetEntry != null && reference.TargetEntry.Metadata.IsOwned() &&
        (reference.TargetEntry.State == EntityState.Added || reference.TargetEntry.State == EntityState.Modified));
}