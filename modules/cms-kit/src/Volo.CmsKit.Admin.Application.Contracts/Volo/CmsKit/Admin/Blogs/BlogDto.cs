using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
