using System;

namespace Volo.Blogging.Blogs.Dtos
{
    public class UpdateBlogDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public string Facebook { get; set; }

        public string Twitter { get; set; }

        public string Instagram { get; set; }

        public string Github { get; set; }

        public string StackOverflow { get; set; }
    }
}