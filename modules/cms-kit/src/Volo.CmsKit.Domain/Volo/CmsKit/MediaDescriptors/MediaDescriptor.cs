using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.MediaDescriptors.Extensions;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptor : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public string Name { get; protected set; }

        public string MimeType { get; protected set; }

        public long Size { get; protected set; }

        protected MediaDescriptor()
        {
            
        }

        public MediaDescriptor(Guid id, string name, string mimeType, long size, Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            
            MimeType = Check.NotNullOrWhiteSpace(mimeType, nameof(name), MediaDescriptorConsts.MaxMimeTypeLength);
            Size = size;
            
            SetName(name);
        }

        public void SetName(string name)
        {
            if (!name.IsValidMediaFileName())
            {
                throw new InvalidMediaDescriptorNameException(name);
            }

            Name = name;
        }
    }
}