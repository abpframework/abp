using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Volo.Abp.Auditing;

public class AuditPropertySetter : IAuditPropertySetter, ITransientDependency
{
    protected ICurrentUser CurrentUser { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IClock Clock { get; }

    public AuditPropertySetter(
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        IClock clock)
    {
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        Clock = clock;
    }

    public virtual void SetCreationProperties(object targetObject)
    {
        SetCreationTime(targetObject);
        SetCreatorId(targetObject);
    }

    public virtual void SetModificationProperties(object targetObject)
    {
        SetLastModificationTime(targetObject);
        SetLastModifierId(targetObject);
    }

    public virtual void SetDeletionProperties(object targetObject)
    {
        SetDeletionTime(targetObject);
        SetDeleterId(targetObject);
    }

    protected virtual void SetCreationTime(object targetObject)
    {
        if (!(targetObject is IHasCreationTime objectWithCreationTime))
        {
            return;
        }

        if (objectWithCreationTime.CreationTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithCreationTime, x => x.CreationTime, () => Clock.Now);
        }
    }

    protected virtual void SetCreatorId(object targetObject)
    {
        if (!CurrentUser.Id.HasValue)
        {
            return;
        }

        if (targetObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser.TenantId)
            {
                return;
            }
        }

        /* TODO: The code below is from old ABP, not implemented yet
            if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
            {
                //Tenant user created a host entity
                return;
            }
             */

        if (targetObject is IMayHaveCreator mayHaveCreatorObject)
        {
            if (mayHaveCreatorObject.CreatorId.HasValue && mayHaveCreatorObject.CreatorId.Value != default)
            {
                return;
            }

            ObjectHelper.TrySetProperty(mayHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id);
        }
        else if (targetObject is IMustHaveCreator mustHaveCreatorObject)
        {
            if (mustHaveCreatorObject.CreatorId != default)
            {
                return;
            }

            ObjectHelper.TrySetProperty(mustHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id.Value);
        }
    }

    protected virtual void SetLastModificationTime(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
        {
            objectWithModificationTime.LastModificationTime = Clock.Now;
        }
    }

    protected virtual void SetLastModifierId(object targetObject)
    {
        if (!(targetObject is IModificationAuditedObject modificationAuditedObject))
        {
            return;
        }

        if (!CurrentUser.Id.HasValue)
        {
            modificationAuditedObject.LastModifierId = null;
            return;
        }

        if (modificationAuditedObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser.TenantId)
            {
                modificationAuditedObject.LastModifierId = null;
                return;
            }
        }

        /* TODO: The code below is from old ABP, not implemented yet
        if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
        {
            //Tenant user modified a host entity
            modificationAuditedObject.LastModifierId = null;
            return;
        }
         */

        modificationAuditedObject.LastModifierId = CurrentUser.Id;
    }

    protected virtual void SetDeletionTime(object targetObject)
    {
        if (targetObject is IHasDeletionTime objectWithDeletionTime)
        {
            if (objectWithDeletionTime.DeletionTime == null)
            {
                objectWithDeletionTime.DeletionTime = Clock.Now;
            }
        }
    }

    protected virtual void SetDeleterId(object targetObject)
    {
        if (!(targetObject is IDeletionAuditedObject deletionAuditedObject))
        {
            return;
        }

        if (deletionAuditedObject.DeleterId != null)
        {
            return;
        }

        if (!CurrentUser.Id.HasValue)
        {
            deletionAuditedObject.DeleterId = null;
            return;
        }

        if (deletionAuditedObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser.TenantId)
            {
                deletionAuditedObject.DeleterId = null;
                return;
            }
        }

        deletionAuditedObject.DeleterId = CurrentUser.Id;
    }
}
