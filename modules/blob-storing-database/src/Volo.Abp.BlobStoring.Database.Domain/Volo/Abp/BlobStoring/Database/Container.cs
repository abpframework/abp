using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class Container : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; }

        public string Name { get; set; }
    }
}