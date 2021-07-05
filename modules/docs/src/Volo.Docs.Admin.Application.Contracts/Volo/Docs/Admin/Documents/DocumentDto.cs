using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Admin.Documents
{
    [Serializable]
    public class DocumentDto : EntityDto<Guid>
    {
        public virtual Guid ProjectId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Version { get; set; }

        public virtual string LanguageCode { get; set; }

        public virtual string FileName { get; set; }
        
        public virtual string Format { get; set; }
        
        public virtual DateTime CreationTime { get; set; }

        public virtual DateTime LastUpdatedTime { get; set; }

        public virtual DateTime? LastSignificantUpdateTime { get; set; }

        public virtual DateTime LastCachedTime { get; set; }
    }
}