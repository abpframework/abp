using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentDto : FullAuditedEntityDto<Guid>
    {
        public Guid RepliedCommentId { get; set; }

        public string Text { get; set; }
    }
}
