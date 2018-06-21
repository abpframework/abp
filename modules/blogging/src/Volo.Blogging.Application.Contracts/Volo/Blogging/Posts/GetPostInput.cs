using System;

namespace Volo.Blogging.Posts
{
    public class GetPostInput
    {
        public string Title { get; set; }

        public Guid BlogId { get; set; }
    }
}