using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Posts
{
    public class PostDto : EntityDto<Guid>
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
