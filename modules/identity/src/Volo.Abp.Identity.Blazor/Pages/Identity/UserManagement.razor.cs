using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement.Blazor.Components;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class UserManagement
    {
        private const string PermissionProviderName = "U";

        private PermissionManagementModal PermissionManagementModal;

        protected IReadOnlyList<IdentityRoleDto> Roles;

        protected AssignedRoleViewModel[] NewEntityRoles;
        
        protected AssignedRoleViewModel[] EditEntityRoles;
        
        public UserManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            Roles = (await AppService.GetAssignableRolesAsync()).Items;
        }

        protected override void OpenCreateModal()
        {
            
            NewEntityRoles = Roles.Select(x => new AssignedRoleViewModel
                            {
                                Name = x.Name,
                                IsAssigned = x.IsDefault
                            }).ToArray();
            
            base.OpenCreateModal();
        }

        protected override Task CreateEntityAsync()
        {
            NewEntity.RoleNames = NewEntityRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
            
            return base.CreateEntityAsync();
        }

        protected override async Task OpenEditModalAsync(Guid id)
        {
            var userRoleNames = (await AppService.GetRolesAsync(id)).Items.Select(r => r.Name).ToList();
            
            EditEntityRoles = Roles.Select(x => new AssignedRoleViewModel
                                {
                                    Name = x.Name,
                                    IsAssigned = userRoleNames.Contains(x.Name)
                                }).ToArray();
            
            await base.OpenEditModalAsync(id);
        }

        protected override Task UpdateEntityAsync()
        {
            EditingEntity.RoleNames = EditEntityRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
            
            return base.UpdateEntityAsync();
        }
    }
    
    public class AssignedRoleViewModel
    {
        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
