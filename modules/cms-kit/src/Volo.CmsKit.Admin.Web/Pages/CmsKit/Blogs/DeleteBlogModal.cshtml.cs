using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Admin.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs;

public class DeleteBlogModal : CmsKitAdminPageModel
{
    [BindProperty]
    public BlogInfoModel Blog { get; set; }

    protected IBlogAdminAppService BlogAdminAppService { get; }

    public DeleteBlogModal(IBlogAdminAppService blogAdminAppService)
    {
        BlogAdminAppService = blogAdminAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var blog = await BlogAdminAppService.GetAsync(id);
        var allBlogs = await BlogAdminAppService.GetAllListAsync();

        Blog = new BlogInfoModel
        {
            Id = blog.Id,
            Name = blog.Name,
            BlogPostCount = blog.BlogPostCount,
            OtherBlogs = allBlogs.Items.Where(b => b.Id != blog.Id).Select(e => new KeyValuePair<Guid, string>(e.Id, e.Name)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await BlogAdminAppService.MoveAllBlogPostsAsync(Blog.Id, Blog.AssignToBlogId);
        await BlogAdminAppService.DeleteAsync(Blog.Id);
        return NoContent();
    }
    
    public class BlogInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int BlogPostCount { get; set; }

        public List<KeyValuePair<Guid, string>> OtherBlogs { get; set; }

        public Guid? AssignToBlogId { get; set; }
    }
}