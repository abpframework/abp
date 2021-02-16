using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogFeatureDto
    {
        [Required]
        public string FeatureName { get; set; }
        public bool Enabled { get; set; }
    }
}
