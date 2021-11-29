using System;

namespace Volo.CmsKit.Public.Ratings
{
    [Serializable]
    public class RatingWithStarCountDto
    {
        public short StarCount { get; set; }

        public int Count { get; set; }

        public bool IsSelectedByCurrentUser { get; set; }
    }
}