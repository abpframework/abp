using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public abstract class RoleManagementBase : BlazoriseCrudPageBase<IIdentityRoleAppService,IdentityRoleDto, Guid, PagedAndSortedResultRequestDto, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;

        public RoleManagementBase()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
        }
    }
}
