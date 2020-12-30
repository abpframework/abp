using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Tags
{
    public class TagUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
