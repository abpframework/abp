using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Members;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blogs.Members;

public class IndexModel : AbpPageModel
{
    private readonly IPostAppService _postAppService;
    
    private readonly IMemberAppService _memberAppService;
    
    private readonly IBlogAppService _blogAppService; 
    
    private readonly BloggingUrlOptions _blogOptions;
    
    public BlogUserDto User { get; set; }
    public List<PostWithDetailsDto> Posts { get; set; }

    public Dictionary<Guid, string> BlogShortNameMap { get; set; }
    
    [BindProperty]
    public CustomIdentityBlogUserUpdateDto CustomUserUpdate { get; set; }
    
    public IndexModel(IPostAppService postAppService, IMemberAppService memberAppService, IBlogAppService blogAppService, IOptions<BloggingUrlOptions> blogOptions)
    {
        _postAppService = postAppService;
        _memberAppService = memberAppService;
        _blogAppService = blogAppService;
        _blogOptions = blogOptions.Value;
    }

    public async Task<IActionResult> OnGetAsync(string userName)
    {
        User = await _memberAppService.FindAsync(userName);

        if (User is null)
        {
            return Redirect("/");
        }
        
        Posts = await _postAppService.GetListByUserIdAsync(User.Id);

        var blogIds = Posts.Select(x => x.BlogId).Distinct();
        BlogShortNameMap = new Dictionary<Guid, string>();

        foreach (var blogId in blogIds)
        {
            BlogShortNameMap[blogId] = (await _blogAppService.GetAsync(blogId)).ShortName;
        }
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await _memberAppService.UpdateUserProfileAsync(CustomUserUpdate);
        return RedirectToPage("/Blogs/Members/Index", new { userName = CurrentUser.UserName });
    }
    
    public string GetBlogPostUrl(PostWithDetailsDto post)
    {
        if (_blogOptions.SingleBlogMode.Enabled)
        {
            return Url.Page("/Blogs/Posts/Detail", new { postUrl = post.Url });
        }
        var blogShortName = BlogShortNameMap[post.BlogId];
        

        return Url.Page("/Blogs/Posts/Detail", new { blogShortName = blogShortName, postUrl = post.Url });
    }
    
    public string GetMemberProfileUrl(BlogUserDto user)
    {
        return Url.Page("/Blogs/Members/Index", new { userName = user.UserName });
    }
    
}