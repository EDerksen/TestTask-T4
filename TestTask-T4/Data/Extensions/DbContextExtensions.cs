using Microsoft.EntityFrameworkCore;

namespace TestTask_T4.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void SetCreatedAt<Tdb>(this Tdb context) where Tdb : DbContext
        {
            var addedEntities = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added);
            foreach (var addedEntity in addedEntities)
            {
                if (addedEntity.Entity is IEntityCreatedAt created)
                {
                    created.CreatedAt = DateTime.UtcNow;
                }

                if (addedEntity.Entity is IEntityModifiedAt modified)
                {
                    modified.ModifiedAt = DateTime.UtcNow;
                }
            }
        }

        public static void SetUpdatedAt<Tdb>(this Tdb context) where Tdb : DbContext
        {
            var modifiedEntities = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified);
            foreach (var modifiedEntity in modifiedEntities)
            {
                if (modifiedEntity.Entity is IEntityModifiedAt modified)
                {
                    modified.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
