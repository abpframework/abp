using System;

namespace Volo.Blogging.Posts
{
    public class PostChangedEvent
    {
        public Guid BlogId { get; set; }
    }
}