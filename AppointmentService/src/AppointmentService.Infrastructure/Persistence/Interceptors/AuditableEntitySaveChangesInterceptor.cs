using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderService.Domain.Abstractions;

namespace OrderService.Infrastructure.Persistence.Interceptors
{
    public sealed class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        public AuditableEntitySaveChangesInterceptor() { }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public static void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "system";
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.LastModifiedBy = "system";
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    entry.Entity.IsDeleted = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = "system";
                    entry.Entity.LastModifiedAt = DateTime.Now;
                }
            }
        }
    }
}
