using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.FeatureManagement.Blazor.Components;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement.Blazor.Pages.TenantManagement
{
    public partial class TenantManagement
    {
        protected const string FeatureProviderName = "T";

        protected bool HasManageConnectionStringsPermission;
        protected bool HasManageFeaturesPermission;
        protected string ManageConnectionStringsPolicyName;
        protected string ManageFeaturesPolicyName;

        protected FeatureManagementModal FeatureManagementModal;

        protected Modal ManageConnectionStringModal;
        protected Validations ManageConnectionStringValidations;

        protected TenantInfoModel TenantInfo;

        public TenantManagement()
        {
            LocalizationResource = typeof(AbpTenantManagementResource);
            ObjectMapperContext = typeof(AbpTenantManagementBlazorModule);

            CreatePolicyName = TenantManagementPermissions.Tenants.Create;
            UpdatePolicyName = TenantManagementPermissions.Tenants.Update;
            DeletePolicyName = TenantManagementPermissions.Tenants.Delete;
            ManageConnectionStringsPolicyName = TenantManagementPermissions.Tenants.ManageConnectionStrings;
            ManageFeaturesPolicyName = TenantManagementPermissions.Tenants.ManageFeatures;

            TenantInfo = new TenantInfoModel();
        }

        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManageConnectionStringsPermission = await AuthorizationService.IsGrantedAsync(ManageConnectionStringsPolicyName);
            HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
        }

        protected virtual async Task OpenEditConnectionStringModalAsync(TenantDto entity)
        {
            ManageConnectionStringValidations.ClearAll();

            var tenantConnectionString = await AppService.GetDefaultConnectionStringAsync(entity.Id);

            TenantInfo = new TenantInfoModel
            {
                Id = entity.Id,
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
            if (ManageConnectionStringValidations.ValidateAll())
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

        [Required]
        public string DefaultConnectionString { get; set; }
    }
}