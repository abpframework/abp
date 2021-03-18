using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.FeatureManagement.Blazor.Components;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement.Blazor.Pages.TenantManagement
{
    public partial class TenantManagement
    {
        protected const string FeatureProviderName = "T";

        protected bool HasManageFeaturesPermission;
        protected string ManageFeaturesPolicyName;

        protected FeatureManagementModal FeatureManagementModal;

        public TenantManagement()
        {
            LocalizationResource = typeof(AbpTenantManagementResource);
            ObjectMapperContext = typeof(AbpTenantManagementBlazorModule);

            CreatePolicyName = TenantManagementPermissions.Tenants.Create;
            UpdatePolicyName = TenantManagementPermissions.Tenants.Update;
            DeletePolicyName = TenantManagementPermissions.Tenants.Delete;

            ManageFeaturesPolicyName = TenantManagementPermissions.Tenants.ManageFeatures;
        }

        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
        }

        protected override string GetDeleteConfirmationMessage(TenantDto entity)
        {
            return string.Format(L["TenantDeletionConfirmationMessage"], entity.Name);
        }
    }
}
