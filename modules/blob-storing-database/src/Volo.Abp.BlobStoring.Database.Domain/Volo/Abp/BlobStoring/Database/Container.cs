using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class Container : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; }

        public string Name { get; set; }

        public Container(Guid id, [NotNull]string name, Guid? tenantId = null) : base(id)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name), ContainerConsts.MaxNameLength);

            TenantId = tenantId;
            Name = name;
        }
    }
}