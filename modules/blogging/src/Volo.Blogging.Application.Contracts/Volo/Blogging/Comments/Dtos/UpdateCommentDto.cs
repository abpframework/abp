using System.ComponentModel.DataAnnotations;

namespace Volo.Blogging.Comments.Dtos
{
    public class UpdateCommentDto
    {
        [Required]
        public string Text { get; set; }
    }
}
