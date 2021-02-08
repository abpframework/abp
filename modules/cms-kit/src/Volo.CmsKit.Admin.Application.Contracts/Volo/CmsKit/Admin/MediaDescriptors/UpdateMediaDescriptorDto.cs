using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public class UpdateMediaDescriptorDto
    {
        [Required]
        [DynamicStringLength(typeof(MediaDescriptorConsts), nameof(MediaDescriptorConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}