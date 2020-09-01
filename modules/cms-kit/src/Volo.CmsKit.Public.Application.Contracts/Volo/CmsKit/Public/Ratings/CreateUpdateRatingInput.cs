using System.ComponentModel.DataAnnotations;
using Volo.CmsKit.Ratings;

namespace Volo.CmsKit.Public.Ratings
{
    public class CreateUpdateRatingInput
    {
        [Required, Range(RatingConsts.MinStarCount, RatingConsts.MaxStarCount)]
        public short StarCount { get; set; }
    }
}