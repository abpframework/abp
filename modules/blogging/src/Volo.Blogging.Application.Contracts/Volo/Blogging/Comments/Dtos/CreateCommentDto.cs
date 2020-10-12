using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Blogging.Comments.Dtos
{
    public class CreateCommentDto
    {
        public Guid? RepliedCommentId { get; set; }

        public Guid PostId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
