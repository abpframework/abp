using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Posts
{
    public class PostDto : FullAuditedEntityDto<Guid>
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string CoverImage { get; protected set; }

        public string Url { get; set; }

        public int ReadCount { get; set; }

        public string Content { get; set; }
    }
}
