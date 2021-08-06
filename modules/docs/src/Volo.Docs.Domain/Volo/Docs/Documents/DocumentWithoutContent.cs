using System;

namespace Volo.Docs.Documents
{
    public class DocumentWithoutContent
    {
        public Guid Id { get; set; }
        
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