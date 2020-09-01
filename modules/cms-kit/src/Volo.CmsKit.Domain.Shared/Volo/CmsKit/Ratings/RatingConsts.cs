using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Ratings
{
    public static class RatingConsts
    {
        public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

        public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

        public const int MaxStarCount = 5;

        public const int MinStarCount = 1;
    }
}