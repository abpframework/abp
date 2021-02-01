using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogDto : EntityDto<Guid>
    {
        [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
