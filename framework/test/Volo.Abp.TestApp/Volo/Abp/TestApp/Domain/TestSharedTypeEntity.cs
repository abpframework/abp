using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Domain;

public class TestSharedEntity : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
{
    private readonly Dictionary<string, object> _dynamicPropertites = new();

    public object this[string key]
    {
        get => _dynamicPropertites.GetValueOrDefault(key);
        set => _dynamicPropertites[key] = value;
    }

    public Guid? TenantId { get; set; }

    public virtual string Name { get; set; }

    public virtual int Age { get; set; }

    public virtual DateTime? Birthday { get; set; }

    public bool IsDeleted { get; set; }

    public TestSharedEntity()
    {

    }

    public TestSharedEntity(Guid id)
        : base(id)
    {

    }
}
