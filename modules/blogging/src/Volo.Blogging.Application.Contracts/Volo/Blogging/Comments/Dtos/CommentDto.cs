using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentDto : FullAuditedEntityDto<Guid>
    {
        public Guid RepliedCommentId { get; set; }

        public string Text { get; set; }
    }
}
