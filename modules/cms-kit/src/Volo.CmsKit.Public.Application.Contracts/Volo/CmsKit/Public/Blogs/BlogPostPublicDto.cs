using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Blogs
{
    public class BlogPostPublicDto : AuditedEntityWithUserDto<Guid, CmsUserDto>
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string UrlSlug { get; set; }

        public string ShortDescription { get; set; }
    }
}
