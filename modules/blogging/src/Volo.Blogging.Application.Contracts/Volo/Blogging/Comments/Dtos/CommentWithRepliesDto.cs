using System.Collections.Generic;

namespace Volo.Blogging.Comments.Dtos
{
    public class CommentWithRepliesDto
    {
        public CommentWithDetailsDto Comment { get; set; }

        public List<CommentWithDetailsDto> Replies { get; set; } = new List<CommentWithDetailsDto>();
    }
}
