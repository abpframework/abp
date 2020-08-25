using System;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Ratings
{
    public class Rating : BasicAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator
    {
        public virtual short StarCount { get; protected set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual Guid CreatorId { get; set; }

        protected Rating()
        {
            
        }
        
        public Rating(
            Guid id,
            [NotNull] short starCount, 
            Guid creatorId
        )
            : base(id)
        {
            StarCount = starCount;
            CreatorId = creatorId;
        }
    }
}