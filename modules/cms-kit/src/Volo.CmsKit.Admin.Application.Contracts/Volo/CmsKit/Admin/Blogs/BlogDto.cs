using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogDto : EntityDto<Guid>
    {
        [Required]
        [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxSlugLength))]
        public string Slug { get; set; }
    }
}
