using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Acme.BookStore.BookManagement.Pages
{
    public class IndexModel : BookManagementPageModel
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