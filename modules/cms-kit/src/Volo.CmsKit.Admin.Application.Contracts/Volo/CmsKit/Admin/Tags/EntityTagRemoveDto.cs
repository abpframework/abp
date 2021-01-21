using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Tags
{
    public class EntityTagRemoveDto
    {
        [Required]
        public Guid TagId { get; set; }

        [Required]
        public string EntityType { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}
