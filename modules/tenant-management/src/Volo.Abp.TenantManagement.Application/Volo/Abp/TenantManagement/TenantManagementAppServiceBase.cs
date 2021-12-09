using Volo.Abp.Application.Services;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement;

public abstract class TenantManagementAppServiceBase : ApplicationService
{
    protected TenantManagementAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpTenantManagementApplicationModule);
        LocalizationResource = typeof(AbpTenantManagementResource);
    }
}
