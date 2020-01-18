using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public abstract class TenantManagementPageModel : AbpPageModel
    {
        public TenantManagementPageModel()
        {
            ObjectMapperContext = typeof(AbpTenantManagementWebModule);
        }
    }
}