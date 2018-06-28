using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Blogging.Posts
{
    public class CreatePostDto
    {
        public Guid BlogId { get; set; }

        [Required]
        [StringLength(PostConsts.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(PostConsts.MaxContentLength)]
        public string Content { get; set; }
    }
}
