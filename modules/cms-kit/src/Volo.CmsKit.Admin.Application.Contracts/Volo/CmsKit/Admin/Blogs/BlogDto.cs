using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    [Serializable]
    public class BlogDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
