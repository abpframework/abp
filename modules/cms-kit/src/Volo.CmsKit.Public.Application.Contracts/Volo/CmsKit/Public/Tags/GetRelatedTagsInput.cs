using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Public.Tags
{
    public class GetRelatedTagsInput
    {
        [Required]
        public string EntityType { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}
