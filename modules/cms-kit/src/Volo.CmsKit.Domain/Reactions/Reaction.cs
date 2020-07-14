using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Reactions
{
    public class Reaction : Entity<Guid>, IAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator
    {
        public string EntityName { get; set; } //TODO: int or create a more common EntityTypes entity/table/...

        public string EntityId { get; set; }

        public string ReactionType { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid CreatorId { get; set; }
    }
}
