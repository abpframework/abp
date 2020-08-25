using System;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Ratings
{
    public class Rating : BasicAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator
    {
        public virtual short Star { get; protected set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual Guid CreatorId { get; set; }

        protected Rating()
        {
            
        }
        
        public Rating(
            Guid id,
            [NotNull] short star, 
            Guid creatorId
        )
            : base(id)
        {
            Star = star;
            CreatorId = creatorId;
        }
    }
}