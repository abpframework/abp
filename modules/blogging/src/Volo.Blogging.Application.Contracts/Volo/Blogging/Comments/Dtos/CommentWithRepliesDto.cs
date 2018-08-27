using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentWithRepliesDto
    {
        public CommentDto Comment { get; set; }

        public List<CommentDto> Replies { get; set; } = new List<CommentDto>();
    }
}
