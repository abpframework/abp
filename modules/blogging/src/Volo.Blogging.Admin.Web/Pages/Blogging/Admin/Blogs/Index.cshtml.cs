using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs
{
    public class IndexModel : BloggingAdminPageModel
    {
        private readonly IAuthorizationService _authorization;

        public IndexModel(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Blogs.Management))
            {
                return Redirect("/");
            }

            return Page();
        }
    }
}
