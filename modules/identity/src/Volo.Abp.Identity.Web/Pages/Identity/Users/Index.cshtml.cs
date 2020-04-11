using System.Threading.Tasks;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class IndexModel : IdentityPageModel
    {
        public virtual Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnPostAsync()
        {
            return Task.CompletedTask;
        }
    }
}
