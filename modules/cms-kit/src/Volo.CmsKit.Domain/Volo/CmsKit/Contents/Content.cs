using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Contents
{
    public class Content : FullAuditedAggregateRoot<Guid>
    {
        public string EntityType { get; set; }
        
        public string EntityId { get; set; }
        
        public string Value { get; set; }

        protected Content()
        {
            
        }
        public Content(Guid id, [NotNull] string entityType, [NotNull] string entityId, [NotNull] string value) : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), ContentConsts.MaxEntityTypeLength);
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId), ContentConsts.MaxEntityIdLength);
            Value = Check.NotNullOrWhiteSpace(value, nameof(value), ContentConsts.MaxValueLength);
        }
    }
}
