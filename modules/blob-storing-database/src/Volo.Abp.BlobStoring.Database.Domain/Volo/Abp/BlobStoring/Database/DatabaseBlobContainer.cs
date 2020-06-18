using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class DatabaseBlobContainer : AggregateRoot<Guid>, IMultiTenant //TODO: Rename to BlobContainer
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string Name { get; protected set; }

        public DatabaseBlobContainer(Guid id, [NotNull] string name, Guid? tenantId = null) 
            : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), DatabaseContainerConsts.MaxNameLength);
            TenantId = tenantId;
        }
    }
}