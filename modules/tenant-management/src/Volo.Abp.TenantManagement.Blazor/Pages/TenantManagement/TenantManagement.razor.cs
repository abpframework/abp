using System;
using System.Threading.Tasks;
using Volo.Abp.BlazoriseUI;

namespace Volo.Abp.TenantManagement.Blazor.Pages.TenantManagement
{
    public abstract class TenantManagementBase 
        : AbpCrudPageBase<ITenantAppService, TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        protected bool ShouldShowEntityActions;
        
        public TenantManagementBase()
        {
            ObjectMapperContext = typeof(AbpTenantManagementBlazorModule);

            CreatePolicyName = TenantManagementPermissions.Tenants.Create;
            UpdatePolicyName = TenantManagementPermissions.Tenants.Update;
            DeletePolicyName = TenantManagementPermissions.Tenants.Delete;
        }
        
        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            ShouldShowEntityActions = HasUpdatePermission ||
                                      HasDeletePermission;
        }
    }
}