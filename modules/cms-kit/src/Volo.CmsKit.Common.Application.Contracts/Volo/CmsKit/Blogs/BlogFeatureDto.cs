using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Blogs
{
    [Serializable]
    public class BlogFeatureDto : EntityDto<Guid>
    {
        public string FeatureName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
