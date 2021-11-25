using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Posts
{
    public class PostWithDetailsDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string CoverImage { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public int ReadCount { get; set; }

        public int CommentCount { get; set; }

        [CanBeNull]
        public BlogUserDto Writer { get; set; }

        public List<TagDto> Tags { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
