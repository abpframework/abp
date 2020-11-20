using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Ratings;

namespace Volo.CmsKit.Public.Ratings
{
    public class CreateUpdateRatingInput
    {
        [Required]
        [DynamicRange(typeof(RatingConsts), typeof(int), nameof(RatingConsts.MinStarCount), nameof(RatingConsts.MaxStarCount))]
        public short StarCount { get; set; }
    }
}