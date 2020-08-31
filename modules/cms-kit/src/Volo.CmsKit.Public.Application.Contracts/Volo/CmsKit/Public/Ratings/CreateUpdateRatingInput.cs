using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Public.Ratings
{
    public class CreateUpdateRatingInput
    {
        [Required, Range(1, 5)]
        public short StarCount { get; set; }
    }
}