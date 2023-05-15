using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Members;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Members;

public class IndexModel : AbpPageModel
{
    private readonly IPostAppService _postAppService;
    
    private readonly IMemberAppService _memberAppService;
    
    private readonly IBlogAppService _blogAppService; 
    
    public BlogUserDto User { get; set; }
    public List<PostWithDetailsDto> Posts { get; set; }

    public Dictionary<Guid, string> BlogShortNameMap { get; set; }
    
    [BindProperty]
    public CustomIdentityBlogUserUpdateDto CustomUserUpdate { get; set; }
    
    public IndexModel(IPostAppService postAppService, IMemberAppService memberAppService, IBlogAppService blogAppService)
    {
        _postAppService = postAppService;
        _memberAppService = memberAppService;
        _blogAppService = blogAppService;
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

        return Redirect($"/members/{CurrentUser.UserName}");
    }
    
    public string GetBlogPostUrl(PostWithDetailsDto post)
    {
        var blogShortName = BlogShortNameMap[post.BlogId];

        return "/" + blogShortName + "/" + post.Url;
    }
    
    public string GetMemberProfileUrl(BlogUserDto user)
    {
        return "/members/" + user.UserName;
    }
    
}