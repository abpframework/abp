using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs
{
    public class BlogFeatureDto : EntityDto<Guid>
    {
        public string FeatureName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
