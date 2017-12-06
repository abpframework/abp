using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class EditModalModel : AbpPageModel
    {
        public IdentityUserDto EditingUser { get; set; }
        public IdentityUserRoleDto[] Roles { get; set; }

        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityRoleAppService _identityRoleAppService;

        public EditModalModel(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
        {
            _identityUserAppService = identityUserAppService;
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            EditingUser = await _identityUserAppService.GetAsync(id);
            Roles = ObjectMapper.Map<List<IdentityRoleDto>, IdentityUserRoleDto[]>(await _identityRoleAppService.GetAllListAsync());
            await SetAssignedRoles();
        }

        private async Task SetAssignedRoles()
        {
            var userRoleNames = (await _identityUserAppService.GetRolesAsync(EditingUser.Id)).Items.Select(r => r.Name).ToList();
            foreach (var role in Roles)
            {
                if (userRoleNames.Contains(role.Name))
                {
                    role.IsAssigned = true;
                }
            }
        }
    }
}