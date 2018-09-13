using System;

namespace Volo.Blogging.Blogs.Dtos
{
    public class UpdateBlogDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}