using System;

namespace Volo.Blogging.Posts
{
    public class UpdatePostDto
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string CoverImage { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }
    }
}
