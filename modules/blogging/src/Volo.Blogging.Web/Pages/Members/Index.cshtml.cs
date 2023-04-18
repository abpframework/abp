using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Members;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Members;

public class IndexModel : AbpPageModel
{
    private readonly IPostAppService _postAppService;
    
    private readonly IMemberAppService _memberAppService;
    public BlogUserDto User { get; set; }
    public List<PostWithDetailsDto> Posts { get; set; }

    public IndexModel(IPostAppService postAppService, IMemberAppService memberAppService)
    {
        _postAppService = postAppService;
        _memberAppService = memberAppService;
    }

    public async Task<IActionResult> OnGetAsync(string userName)
    {
        User = await _memberAppService.FindAsync(userName);

        if (User is null)
        {
            return Redirect("/abp");
        }

        Posts = await _postAppService.GetListByUserIdAsync(User.Id);

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        return Redirect($"/members/{CurrentUser.UserName}");
    }
    
}