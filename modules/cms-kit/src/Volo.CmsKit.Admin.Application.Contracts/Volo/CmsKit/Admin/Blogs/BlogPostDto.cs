using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    [Serializable]
    public class BlogPostDto : EntityDto<Guid>
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string Content { get; set; }

        public Guid? CoverImageMediaId { get; set; }
    }
}
