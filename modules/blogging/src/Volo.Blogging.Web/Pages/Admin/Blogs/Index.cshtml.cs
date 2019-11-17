using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Pages.Blog;

namespace Volo.Blogging.Pages.Admin.Blogs
{
    public class IndexModel : BloggingPageModel
    {
        private readonly IAuthorizationService _authorization;

        public IndexModel(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Blogs.Management))
            {
                return Redirect("/");
            }

            return Page();
        }
    }
}