using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.FeatureManagement.Blazor.Components;

namespace Volo.Abp.TenantManagement.Blazor.Pages.TenantManagement
{
    public abstract class TenantManagementBase 
        : AbpCrudPageBase<ITenantAppService, TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        protected const string FeatureProviderName = "T";

        protected bool HasManageConnectionStringsPermission;
        protected bool HasManageFeaturesPermission;
        protected string ManageConnectionStringsPolicyName;
        protected string ManageFeaturesPolicyName;

        protected FeatureManagementModal FeatureManagementModal;

        protected Modal ManageConnectionStringModal;

        protected TenantInfoModel TenantInfo;

        public TenantManagementBase()
        {
            ObjectMapperContext = typeof(AbpTenantManagementBlazorModule);

            CreatePolicyName = TenantManagementPermissions.Tenants.Create;
            UpdatePolicyName = TenantManagementPermissions.Tenants.Update;
            DeletePolicyName = TenantManagementPermissions.Tenants.Delete;
            ManageConnectionStringsPolicyName = TenantManagementPermissions.Tenants.ManageConnectionStrings;
            ManageFeaturesPolicyName = TenantManagementPermissions.Tenants.ManageFeatures;

            TenantInfo = new TenantInfoModel();
        }
        
        protected async override Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManageConnectionStringsPermission = await AuthorizationService.IsGrantedAsync(ManageConnectionStringsPolicyName);
            HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
        }

        protected virtual async Task OpenEditConnectionStringModalAsync(Guid id)
        {
            var tenantConnectionString = await AppService.GetDefaultConnectionStringAsync(id);

            TenantInfo = new TenantInfoModel
            {
                Id = id,
                DefaultConnectionString = tenantConnectionString,
                UseSharedDatabase = tenantConnectionString.IsNullOrWhiteSpace()
            };

            ManageConnectionStringModal.Show();
        }

        protected virtual Task CloseEditConnectionStringModal()
        {
            ManageConnectionStringModal.Hide();
            return Task.CompletedTask;
        }

        protected virtual async Task UpdateConnectionStringAsync()
        {
            await CheckPolicyAsync(ManageConnectionStringsPolicyName);

            if (TenantInfo.UseSharedDatabase || TenantInfo.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                await AppService.DeleteDefaultConnectionStringAsync(TenantInfo.Id);
            }
            else
            {
                await AppService.UpdateDefaultConnectionStringAsync(TenantInfo.Id, TenantInfo.DefaultConnectionString);
            }

            ManageConnectionStringModal.Hide();
        }

        protected override string GetDeleteConfirmationMessage(TenantDto entity)
        {
            return string.Format(L["TenantDeletionConfirmationMessage"], entity.Name);
        }
    }

    public class TenantInfoModel
    {
        public Guid Id { get; set; }

        public bool UseSharedDatabase { get; set; }

        public string DefaultConnectionString { get; set; }
    }
}