using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Volo.CmsKit.Pages;

public class IndexModel : CmsKitPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
