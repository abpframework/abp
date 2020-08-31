using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Ratings
{
    public static class RatingConsts
    {
        public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

        public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

        public static int MaxRating { get; set; } = 5;

        public static int MinRating { get; set; } = 0;
    }
}