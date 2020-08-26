using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Public.Ratings
{
    public class CreateRatingInput
    {
        [Required]
        public short StarCount { get; set; }
    }
}