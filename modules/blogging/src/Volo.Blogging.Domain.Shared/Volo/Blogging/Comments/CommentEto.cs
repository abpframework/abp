using System;

namespace Volo.Blogging.Comments
{
    public class CommentEto
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }
    }
}