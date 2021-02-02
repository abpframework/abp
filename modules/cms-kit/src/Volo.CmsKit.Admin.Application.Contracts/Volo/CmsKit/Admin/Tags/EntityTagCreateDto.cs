using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Tags
{
    public class EntityTagCreateDto
    {
        [Required]
        public string TagName { get; set; }

        [Required]
        public string EntityType { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}
