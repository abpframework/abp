using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class CreateModalModel : AbpPageModel
    {
        public IdentityUserDto User { get; set; }

        public IdentityUserRoleDto[] Roles { get; set; }

        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityRoleAppService _identityRoleAppService;

        public CreateModalModel(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
        {
            _identityUserAppService = identityUserAppService;
            _identityRoleAppService = identityRoleAppService;
        }

        public void OnGet()
        {
            User = new IdentityUserDto();
            Roles = ObjectMapper.Map<List<IdentityRoleDto>, IdentityUserRoleDto[]>(
                AsyncHelper.RunSync(() => _identityRoleAppService.GetAllListAsync())
            );
        }
    }
}
