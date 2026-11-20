using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

// The root of a hierarchy that implements neither ISoftDelete nor IMultiTenant.
public class Notice : AggregateRoot<Guid>
{
    public string Text { get; set; }
}
