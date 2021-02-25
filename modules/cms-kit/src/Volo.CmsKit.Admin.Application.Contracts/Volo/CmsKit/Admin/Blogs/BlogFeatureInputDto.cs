using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogFeatureInputDto
    {
        [Required]
        public string FeatureName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
