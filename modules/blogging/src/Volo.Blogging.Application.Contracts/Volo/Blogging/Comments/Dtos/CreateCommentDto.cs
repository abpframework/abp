using System;

namespace Volo.Blogging.Comments.Dtos
{
    public class CreateCommentDto
    {
        public Guid? RepliedCommentId { get; set; }

        public Guid PostId { get; set; }

        public string Text { get; set; }
    }
}