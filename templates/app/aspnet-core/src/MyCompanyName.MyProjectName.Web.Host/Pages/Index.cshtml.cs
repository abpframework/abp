using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace MyCompanyName.MyProjectName.Web.Pages
{
    public class IndexModel : MyProjectNamePageModel
    {
        public void OnGet()
        {
            
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}