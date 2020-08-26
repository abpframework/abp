using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Public.Ratings
{
    public class UpdateRatingInput
    {
        [Required]
        public short StarCount { get; set; }
    }
}