using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Ratings
{
    public class Rating : BasicAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual string EntityType { get; protected set; }

        public virtual string EntityId { get; protected set; }
        
        public virtual short StarCount { get; protected set; }
        
        public virtual Guid CreatorId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        protected Rating()
        {
            
        }
        
        internal Rating(
            Guid id,
            [NotNull] string entityType,
            [NotNull] string entityId,
            short starCount, 
            Guid creatorId,
            Guid? tenantId = null
        )
            : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), RatingConsts.MaxEntityTypeLength);
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId), RatingConsts.MaxEntityIdLength);
            SetStarCount(starCount);
            CreatorId = creatorId;
            TenantId = tenantId;
        }

        public virtual void SetStarCount(short starCount)
        {
            if(starCount <= RatingConsts.MaxStarCount && starCount >= RatingConsts.MinStarCount)
            {
                StarCount = starCount;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Choosen star must between {RatingConsts.MinStarCount} and {RatingConsts.MaxStarCount}");
            }
        }
    }
}