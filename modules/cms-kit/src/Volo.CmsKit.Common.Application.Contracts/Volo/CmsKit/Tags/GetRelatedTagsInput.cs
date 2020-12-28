using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Tags
{
    public class GetRelatedTagsInput
    {
        [Required]
        public string EntityType { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}
