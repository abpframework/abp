using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Volo.Blogging.Posts
{
    public class UpdatePostDto
    {
        public Guid BlogId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string CoverImage { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Content { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }
    }
}
