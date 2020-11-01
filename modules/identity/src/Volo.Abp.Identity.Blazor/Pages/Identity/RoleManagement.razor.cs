using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public abstract class RoleManagementBase : AbpCrudPageBase<IIdentityRoleAppService, IdentityRoleDto, Guid, GetIdentityRolesInput, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;
        protected string ManagePermissionsPolicyName;

        protected bool HasManagePermissionsPermission { get; set; }

        public RoleManagementBase()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
            LocalizationResource = typeof(IdentityResource);

            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
            ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
        }

        protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
        {
            return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
        }
    }
}
