using System;
using Volo.Abp.Application.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentWithDetailsDto : FullAuditedEntityDto<Guid>
    {
        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }

        public BlogUserDto Writer { get; set; }
    }
}
