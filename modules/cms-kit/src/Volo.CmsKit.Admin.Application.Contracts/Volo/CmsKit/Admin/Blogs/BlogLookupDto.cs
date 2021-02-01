using System;

namespace Volo.CmsKit.Admin.Application.Contracts.Volo.CmsKit.Admin.Blogs
{
    public class BlogLookupDto
    {
        public BlogLookupDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
