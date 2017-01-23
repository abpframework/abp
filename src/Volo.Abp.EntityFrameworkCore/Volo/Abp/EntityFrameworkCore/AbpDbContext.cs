using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore
{
    public abstract class AbpDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        protected AbpDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureConcurrencyStamp(entityType);
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();
            ApplyAbpConcepts();

            try
            {
                ChangeTracker.AutoDetectChangesEnabled = false;
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AbpDbConcurrencyException(ex.Message, ex);
            }
            finally
            {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();
            ApplyAbpConcepts();

            try
            {
                ChangeTracker.AutoDetectChangesEnabled = false;
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AbpDbConcurrencyException(ex.Message, ex);
            }
            finally
            {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        protected virtual void ConfigureConcurrencyStamp(IMutableEntityType entityType)
        {
            if (!typeof(IHasConcurrencyStamp).GetTypeInfo().IsAssignableFrom(entityType.ClrType))
            {
                return;
            }

            entityType
                .GetProperties()
                .First(p => p.Name == nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken = true;
        }

        protected virtual void ApplyAbpConcepts()
        {
            /* Implement other concepts from ABP v1.x */

            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        HandleConcurrencyStamp(entry);
                        break;
                }
            }
        }

        protected virtual void HandleConcurrencyStamp(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
            {
                return;
            }

            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
        }
    }
}