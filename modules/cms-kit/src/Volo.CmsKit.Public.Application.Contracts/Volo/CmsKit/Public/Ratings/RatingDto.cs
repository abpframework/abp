using System;

namespace Volo.CmsKit.Public.Ratings
{
    public class RatingDto
    {
        public Guid Id { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }
        
        public short StarCount { get; set; }
        
        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}