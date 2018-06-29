using System;

namespace Volo.Blogging.Posts
{
    public class GetPostInput
    {
        public string Url { get; set; }

        public Guid BlogId { get; set; }
    }
}