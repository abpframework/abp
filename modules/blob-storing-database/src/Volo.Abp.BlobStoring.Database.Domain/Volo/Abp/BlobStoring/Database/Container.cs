using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class Container : AggregateRoot<Guid>, IMultiTenant //TODO: Rename to BlobContainer
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string Name { get; protected set; }

        public Container(Guid id, [NotNull] string name, Guid? tenantId = null) 
            : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), ContainerConsts.MaxNameLength);
            TenantId = tenantId;
        }
    }
}