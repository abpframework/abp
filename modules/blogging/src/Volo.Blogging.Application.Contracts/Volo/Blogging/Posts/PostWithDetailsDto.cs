using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Posts
{
    public class PostWithDetailsDto : FullAuditedEntityDto<Guid>
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
