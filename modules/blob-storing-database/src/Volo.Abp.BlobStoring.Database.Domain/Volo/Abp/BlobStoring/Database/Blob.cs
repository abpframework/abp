using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class Blob : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid ContainerId { get; set; }

        public Guid? TenantId { get; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
        
        public Blob(Guid id, Guid containerId, string name, byte[] content, Guid? tenantId) : base(id)
        {
            ContainerId = containerId;
            Name = name;
            Content = content;
            TenantId = tenantId;
        }
    }
}