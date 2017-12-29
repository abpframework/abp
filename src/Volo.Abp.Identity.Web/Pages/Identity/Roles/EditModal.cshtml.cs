using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class EditModalModel : AbpPageModel
    {
        [BindProperty]
        public RoleInfoModel RoleInfo { get; set; }

        private readonly IIdentityRoleAppService _identityRoleAppService;

        public EditModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var role = await _identityRoleAppService.GetAsync(id);
            RoleInfo =  ObjectMapper.Map<IdentityRoleDto, RoleInfoModel>(role);
        }

        

    }
}