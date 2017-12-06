using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class CreateModalModel : AbpPageModel
    {
        public IdentityUserRoleDto[] Roles { get; set; }

        private readonly IIdentityRoleAppService _identityRoleAppService;

        public CreateModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync()
        {
            Roles = ObjectMapper.Map<List<IdentityRoleDto>, IdentityUserRoleDto[]>(
                await _identityRoleAppService.GetAllListAsync()
            );
        }
    }
}
