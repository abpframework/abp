using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.MongoDB.TestApp.FifthContext;

public class FifthDbContextMultiTenantDummyEntity : AggregateRoot<Guid>, IMultiTenant
{
    public string Value { get; set; }

    public Guid? TenantId { get; set; }
}
