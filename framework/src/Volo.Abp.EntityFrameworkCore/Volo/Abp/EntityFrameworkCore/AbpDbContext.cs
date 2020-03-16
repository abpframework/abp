using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.EntityHistory;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueConverters;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore
{
    public abstract class AbpDbContext<TDbContext> : DbContext, IEfCoreDbContext, ITransientDependency
        where TDbContext : DbContext
    {
        protected virtual Guid? CurrentTenantId => CurrentTenant?.Id;

        protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

        protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

        public ICurrentTenant CurrentTenant { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public IDataFilter DataFilter { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IEntityHistoryHelper EntityHistoryHelper { get; set; }

        public IAuditingManager AuditingManager { get; set; }

        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IClock Clock { get; set; }

        public ILogger<AbpDbContext<TDbContext>> Logger { get; set; }

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
            = typeof(AbpDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureBaseProperties),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );

        private static readonly MethodInfo ConfigureValueConverterMethodInfo
            = typeof(AbpDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureValueConverter),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );

        private static readonly MethodInfo ConfigureValueGeneratedMethodInfo
            = typeof(AbpDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureValueGenerated),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );

        protected AbpDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            EntityHistoryHelper = NullEntityHistoryHelper.Instance;
            Logger = NullLogger<AbpDbContext<TDbContext>>.Instance;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureBasePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueConverterMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueGeneratedMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                var auditLog = AuditingManager?.Current?.Log;

                List<EntityChangeInfo> entityChangeList = null;
                if (auditLog != null)
                {
                    entityChangeList = EntityHistoryHelper.CreateChangeList(ChangeTracker.Entries().ToList());
                }

                var changeReport = ApplyAbpConcepts();

                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                await EntityChangeEventHelper.TriggerEventsAsync(changeReport);

                if (auditLog != null)
                {
                    EntityHistoryHelper.UpdateChangeList(entityChangeList);
                    auditLog.EntityChanges.AddRange(entityChangeList);
                    Logger.LogDebug($"Added {entityChangeList.Count} entity changes to the current audit log");
                }

                return result;
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

        protected virtual EntityChangeReport ApplyAbpConcepts()
        {
            var changeReport = new EntityChangeReport();

            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                ApplyAbpConcepts(entry, changeReport);
            }

            return changeReport;
        }

        protected virtual void ApplyAbpConcepts(EntityEntry entry, EntityChangeReport changeReport)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAbpConceptsForAddedEntity(entry, changeReport);
                    break;
                case EntityState.Modified:
                    ApplyAbpConceptsForModifiedEntity(entry, changeReport);
                    break;
                case EntityState.Deleted:
                    ApplyAbpConceptsForDeletedEntity(entry, changeReport);
                    break;
            }

            AddDomainEvents(changeReport, entry.Entity);
        }

        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry, EntityChangeReport changeReport)
        {
            CheckAndSetId(entry);
            SetConcurrencyStampIfNull(entry);
            SetCreationAuditProperties(entry);
            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry, EntityChangeReport changeReport)
        {
            UpdateConcurrencyStamp(entry);
            SetModificationAuditProperties(entry);

            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry);
                changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
            }
            else
            {
                changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
            }
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry, EntityChangeReport changeReport)
        {
            if (TryCancelDeletionForSoftDelete(entry))
            {
                UpdateConcurrencyStamp(entry);
                SetDeletionAuditProperties(entry);
            }

            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        }

        protected virtual bool IsHardDeleted(EntityEntry entry)
        {
            var hardDeletedEntities = UnitOfWorkManager?.Current?.Items.GetOrDefault(UnitOfWorkItemNames.HardDeletedEntities) as HashSet<IEntity>;
            if (hardDeletedEntities == null)
            {
                return false;
            }

            return hardDeletedEntities.Contains(entry.Entity);
        }

        protected virtual void AddDomainEvents(EntityChangeReport changeReport, object entityAsObj)
        {
            var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
            if (localEvents != null && localEvents.Any())
            {
                changeReport.DomainEvents.AddRange(localEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
            if (distributedEvents != null && distributedEvents.Any())
            {
                changeReport.DistributedEvents.AddRange(distributedEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
                generatesDomainEventsEntity.ClearDistributedEvents();
            }
        }

        protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
            {
                return;
            }

            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
            {
                return;
            }

            if (entity.ConcurrencyStamp != null)
            {
                return;
            }

            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual bool TryCancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return false;
            }

            if (IsHardDeleted(entry))
            {
                return false;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
            return true;
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            if (entry.Entity is IEntity<Guid> entityWithGuidId)
            {
                TrySetGuidId(entry, entityWithGuidId);
            }
        }

        protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
        {
            if (entity.Id != default)
            {
                return;
            }

            var idProperty = entry.Property("Id").Metadata.PropertyInfo;

            //Check for DatabaseGeneratedAttribute
            var dbGeneratedAttr = ReflectionHelper
                .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                    idProperty
                );

            if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
            {
                return;
            }

            EntityHelper.TrySetId(
                entity,
                () => GuidGenerator.Create(),
                true
            );
        }

        protected virtual void SetCreationAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetCreationProperties(entry.Entity);
        }

        protected virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetModificationProperties(entry.Entity);
        }

        protected virtual void SetDeletionAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetDeletionProperties(entry.Entity);
        }

        protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }

            modelBuilder.Entity<TEntity>().ConfigureByConvention();

            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
        }

        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual void ConfigureValueConverter<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.BaseType == null &&
                !typeof(TEntity).IsDefined(typeof(DisableDateTimeNormalizationAttribute), true) &&
                !typeof(TEntity).IsDefined(typeof(OwnedAttribute), true) &&
                !mutableEntityType.IsOwned())
            {
                if (Clock == null || !Clock.SupportsMultipleTimezone)
                {
                    return;
                }

                var dateTimeValueConverter = new AbpDateTimeValueConverter(Clock);

                var dateTimePropertyInfos = typeof(TEntity).GetProperties()
                    .Where(property =>
                        (property.PropertyType == typeof(DateTime) ||
                         property.PropertyType == typeof(DateTime?)) &&
                        property.CanWrite &&
                        !property.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true)
                    ).ToList();

                dateTimePropertyInfos.ForEach(property =>
                {
                    modelBuilder
                        .Entity<TEntity>()
                        .Property(property.Name)
                        .HasConversion(dateTimeValueConverter);
                });
            }
        }

        protected virtual void ConfigureValueGenerated<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (!typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }

            var idPropertyBuilder = modelBuilder.Entity<TEntity>().Property(x => ((IEntity<Guid>)x).Id);
            if (idPropertyBuilder.Metadata.PropertyInfo.IsDefined(typeof(DatabaseGeneratedAttribute), true))
            {
                return;
            }

            idPropertyBuilder.ValueGeneratedNever();
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
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
                expression = e => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(e, "IsDeleted");
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> multiTenantFilter = e => !IsMultiTenantFilterEnabled || EF.Property<Guid>(e, "TenantId") == CurrentTenantId;
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