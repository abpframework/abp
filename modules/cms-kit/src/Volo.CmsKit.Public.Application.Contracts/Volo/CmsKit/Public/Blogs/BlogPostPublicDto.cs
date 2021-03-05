using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Blogs
{    
    [Serializable]
    public class BlogPostPublicDto : AuditedEntityDto<Guid>
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public Guid? CoverImageMediaId { get; set; }

        public CmsUserDto Author { get; set; }
    }
}
