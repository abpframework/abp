using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.GlobalResources;

public class GlobalResource : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual string Name { get; private set; }
    
    public virtual string Value { get; private set; }

    public virtual Guid? TenantId { get; protected set; }
    
    
    protected GlobalResource()
    {
    }

    internal GlobalResource(
        Guid id,
        [NotNull] string name,
        [CanBeNull] string value,
        Guid? tenantId = null) : base(id)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name), GlobalResourceConsts.MaxNameLength);
        Value = Check.Length(value, nameof(value), GlobalResourceConsts.MaxValueLength);
        
        TenantId = tenantId;
    }

    public virtual void SetValue(string value)
    {
        Check.Length(value, nameof(value), GlobalResourceConsts.MaxValueLength);

        Value = value;
    }
}