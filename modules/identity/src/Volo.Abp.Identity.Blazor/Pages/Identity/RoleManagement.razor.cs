using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public abstract class RoleManagementBase : AbpCrudPageBase<IIdentityRoleAppService,IdentityRoleDto, Guid, PagedAndSortedResultRequestDto, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;

        protected bool HasManagePermissionsPermission { get; set; }

        protected bool ShouldShowEntityActions { get; set; }

        protected Validations ValidationsRef { get; set; }

        public RoleManagementBase()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);

            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
        }

        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(
                IdentityPermissions.Roles.ManagePermissions
            );

            ShouldShowEntityActions = HasUpdatePermission ||
                                      HasDeletePermission ||
                                      HasManagePermissionsPermission;
        }

        protected virtual Task OnCreateEntityClicked()
        {
            if ( ValidationsRef.ValidateAll() )
            {
                CreateModal.Hide();

                return CreateEntityAsync();
            }

            return Task.CompletedTask;
        }
    }
}
