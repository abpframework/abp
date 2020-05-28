using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class Blob : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid ContainerId { get; set; }

        public Guid? TenantId { get; }

        public string Name { get; set; }

        public byte[] Content { get; private set; }
        
        public Blob(Guid id, Guid containerId, [NotNull]string name, byte[] content, Guid? tenantId) : base(id)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name), BlobConsts.MaxNameLength);
            CheckContent(content);

            Content = content;
            ContainerId = containerId;
            Name = name;
            TenantId = tenantId;
        }

        public virtual void SetContent(byte[] content)
        {
            CheckContent(content);

            Content = content;
        }

        protected void CheckContent(byte[] content)
        {
            if (content.Length >= BlobConsts.MaxContentLength)
            {
                throw new AbpException("Blob content size cannot be more than 2GB.");
            }
        }
    }
}