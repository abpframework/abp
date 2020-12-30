using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Tags
{
    public class TagCreateDto
    {
        [Required]
        public string EntityType { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
