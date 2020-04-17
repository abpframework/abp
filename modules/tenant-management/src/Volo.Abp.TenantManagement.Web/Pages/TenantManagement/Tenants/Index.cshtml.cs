using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class IndexModel : TenantManagementPageModel
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