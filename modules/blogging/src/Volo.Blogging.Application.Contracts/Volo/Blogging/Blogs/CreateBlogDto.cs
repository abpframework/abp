using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Blogs
{
    public class CreateBlogDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}