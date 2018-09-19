using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Blogging.Pages.Admin.Blogs
{
    public class IndexModel : AbpPageModel
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