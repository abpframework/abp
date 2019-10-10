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
using Newtonsoft.Json;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EntityFrameworkCore.EntityHistory;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

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

        public ILogger<AbpDbContext<TDbContext>> Logger { get; set; }

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
            = typeof(AbpDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureBaseProperties),
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
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //TODO: Reduce duplications with SaveChangesAsync
            //TODO: Instead of adding entity changes to audit log, write them to uow and add to audit log only if uow succeed

            try
            {
                var auditLog = AuditingManager?.Current?.Log;

                List<EntityChangeInfo> entityChangeList = null;
                if (auditLog != null)
                {
                    entityChangeList = EntityHistoryHelper.CreateChangeList(ChangeTracker.Entries().ToList());
                }

                var changeReport = ApplyAbpConcepts();

                var result = base.SaveChanges(acceptAllChangesOnSuccess);

                AsyncHelper.RunSync(() => EntityChangeEventHelper.TriggerEventsAsync(changeReport));

                if (auditLog != null)
                {
                    EntityHistoryHelper.UpdateChangeList(entityChangeList);
                    auditLog.EntityChanges.AddRange(entityChangeList);
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
            CancelDeletionForSoftDelete(entry);
            UpdateConcurrencyStamp(entry);
            SetDeletionAuditProperties(entry);
            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        }

        protected virtual void AddDomainEvents(EntityChangeReport changeReport, object entityAsObj)
        {
            var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            var localEvents = generatesDomainEventsEntity.GetLocalEvents().ToArray();
            if (localEvents.Any())
            {
                changeReport.DomainEvents.AddRange(localEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents().ToArray();
            if (distributedEvents.Any())
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
            ConfigureConcurrencyStampProperty<TEntity>(modelBuilder, mutableEntityType);
            ConfigureExtraProperties<TEntity>(modelBuilder, mutableEntityType);
            ConfigureAuditProperties<TEntity>(modelBuilder, mutableEntityType);
            ConfigureTenantIdProperty<TEntity>(modelBuilder, mutableEntityType);
            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
        }

        protected virtual void ConfigureConcurrencyStampProperty<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (!typeof(IHasConcurrencyStamp).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }

            modelBuilder.Entity<TEntity>(b =>
            {
                b.Property(x => ((IHasConcurrencyStamp) x).ConcurrencyStamp)
                    .IsConcurrencyToken()
                    .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
            });
        }

        protected virtual void ConfigureExtraProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (!typeof(IHasExtraProperties).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }

            modelBuilder.Entity<TEntity>(b =>
            {
                b.Property(x => ((IHasExtraProperties) x).ExtraProperties)
                    .HasConversion(
                        d => JsonConvert.SerializeObject(d, Formatting.None),
                        s => JsonConvert.DeserializeObject<Dictionary<string, object>>(s)
                    )
                    .HasColumnName(nameof(IHasExtraProperties.ExtraProperties));
            });
        }

        protected virtual void ConfigureAuditProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (typeof(TEntity).IsAssignableTo<IHasCreationTime>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IHasCreationTime)x).CreationTime)
                        .IsRequired()
                        .HasColumnName(nameof(IHasCreationTime.CreationTime));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IMayHaveCreator>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IMayHaveCreator)x).CreatorId)
                        .IsRequired(false)
                        .HasColumnName(nameof(IMayHaveCreator.CreatorId));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IMustHaveCreator>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IMustHaveCreator)x).CreatorId)
                        .IsRequired()
                        .HasColumnName(nameof(IMustHaveCreator.CreatorId));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IHasModificationTime>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IHasModificationTime)x).LastModificationTime)
                        .IsRequired(false)
                        .HasColumnName(nameof(IHasModificationTime.LastModificationTime));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IModificationAuditedObject>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IModificationAuditedObject)x).LastModifierId)
                        .IsRequired(false)
                        .HasColumnName(nameof(IModificationAuditedObject.LastModifierId));
                });
            }

            if (typeof(TEntity).IsAssignableTo<ISoftDelete>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((ISoftDelete) x).IsDeleted)
                        .IsRequired()
                        .HasDefaultValue(false)
                        .HasColumnName(nameof(ISoftDelete.IsDeleted));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IHasDeletionTime>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IHasDeletionTime)x).DeletionTime)
                        .IsRequired(false)
                        .HasColumnName(nameof(IHasDeletionTime.DeletionTime));
                });
            }

            if (typeof(TEntity).IsAssignableTo<IDeletionAuditedObject>())
            {
                modelBuilder.Entity<TEntity>(b =>
                {
                    b.Property(x => ((IDeletionAuditedObject)x).DeleterId)
                        .IsRequired(false)
                        .HasColumnName(nameof(IDeletionAuditedObject.DeleterId));
                });
            }
        }

        protected virtual void ConfigureTenantIdProperty<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (!typeof(TEntity).IsAssignableTo<IMultiTenant>())
            {
                return;
            }

            modelBuilder.Entity<TEntity>(b =>
            {
                b.Property(x => ((IMultiTenant)x).TenantId)
                    .IsRequired(false)
                    .HasColumnName(nameof(IMultiTenant.TenantId));
            });
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