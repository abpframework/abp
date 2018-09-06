using System.Collections.Generic;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentWithRepliesDto
    {
        public CommentDto Comment { get; set; }

        public List<CommentDto> Replies { get; set; } = new List<CommentDto>();
    }
}
