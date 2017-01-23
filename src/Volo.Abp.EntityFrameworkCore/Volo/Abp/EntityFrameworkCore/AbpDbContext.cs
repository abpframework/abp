using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AbpDbConcurrencyException(ex.Message, ex);
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AbpDbConcurrencyException(ex.Message, ex);
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
    }
}