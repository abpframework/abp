using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Ratings
{
    public static class RatingConsts
    {
        public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

        public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

        public static int MaxStarCount { get; set; } = 5;

        public static int MinStarCount { get; set; } = 1;
    }
}