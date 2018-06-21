using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;

namespace Volo.Blogging.Pages.Blog
{
    public class IndexModel : AbpPageModel
    {
        public List<BlogDto> Blogs { get; private set; }

        public IndexModel()
        {
            
        }

        public async Task OnGet()
        {
            Blogs = new List<BlogDto>
            {
                new BlogDto {Id = Guid.NewGuid(), Name = "abp", ShortName = "abp", Description = "a b p"}
            };
        }
    }
}