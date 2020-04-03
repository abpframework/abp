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

        [Required]
        public string CoverImage { get; set; }

        [Required]
        [StringLength(PostConsts.MaxUrlLength)]
        public string Url { get; set; }

        [StringLength(PostConsts.MaxContentLength)]
        public string Content { get; set; }

        public string Tags { get; set; }

        [StringLength(PostConsts.MaxDescriptionLength)]
        public string Description { get; set; }

    }
}
