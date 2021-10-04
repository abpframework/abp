using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentWithDetailsDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }

        public BlogUserDto Writer { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
