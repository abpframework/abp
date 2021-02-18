using JetBrains.Annotations;
using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogFeatureDto : EntityDto<Guid>
    {
        [NotNull]
        public string FeatureName { get; set; }
        public bool Enabled { get; set; }
    }
}
