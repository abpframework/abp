using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore
{
    public abstract class AbpDbContext<TDbContext> : DbContext, IEfCoreDbContext
        where TDbContext : DbContext
    {
        public Guid? CurrentTenantId => CurrentTenant?.Id;

        protected virtual bool IsMayHaveTenantFilterEnabled => DataFilter.IsEnabled<IMultiTenant>();
        protected virtual bool IsSoftDeleteFilterEnabled => DataFilter.IsEnabled<ISoftDelete>();

        public ICurrentTenant CurrentTenant { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public IDataFilter DataFilter { get; set; }

        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(AbpDbContext<TDbContext>).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        protected AbpDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureConcurrencyStamp(entityType);

                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
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

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
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
                    case EntityState.Added:
                        CheckAndSetId(entry);
                        break;
                    case EntityState.Modified:
                        HandleConcurrencyStamp(entry);
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
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

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //Set GUID Ids
            var entity = entry.Entity as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                var dbGeneratedAttr = ReflectionHelper
                    .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                    );

                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    entity.Id = GuidGenerator.Create();
                }
            }
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */

                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != IsSoftDeleteFilterEnabled;
                expression = expression == null ? softDeleteFilter : CombineExpressions(expression, softDeleteFilter);
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                Expression<Func<TEntity, bool>> multiTenantFilter = e => ((IMultiTenant)e).TenantId == CurrentTenantId || (((IMultiTenant)e).TenantId == CurrentTenantId) == IsMayHaveTenantFilterEnabled;
                expression = expression == null ? multiTenantFilter : CombineExpressions(expression, multiTenantFilter);
            }
            
            return expression;
        }

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }
    }
}