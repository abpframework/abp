using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public abstract class RoleManagementBase : AbpCrudPageBase<IIdentityRoleAppService, IdentityRoleDto, Guid, PagedAndSortedResultRequestDto, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;

        protected bool HasManagePermissionsPermission { get; set; }

        protected bool ShouldShowEntityActions { get; set; }

        protected Validations CreateValidationsRef { get; set; }

        protected Validations EditValidationsRef { get; set; }

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

        protected override Task OpenCreateModalAsync()
        {
            CreateValidationsRef.ClearAll();

            return base.OpenCreateModalAsync();
        }

        protected override Task OpenEditModalAsync(Guid id)
        {
            EditValidationsRef.ClearAll();

            return base.OpenEditModalAsync(id);
        }

        protected override Task CreateEntityAsync()
        {
            if (CreateValidationsRef.ValidateAll())
            {
                CreateModal.Hide();

                return base.CreateEntityAsync();
            }

            return Task.CompletedTask;
        }

        protected override Task UpdateEntityAsync()
        {
            if (EditValidationsRef.ValidateAll())
            {
                EditModal.Hide();

                return base.UpdateEntityAsync();
            }

            return Task.CompletedTask;
        }
    }
}
