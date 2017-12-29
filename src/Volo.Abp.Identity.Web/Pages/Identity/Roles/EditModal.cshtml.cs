using System;
using System.ComponentModel.DataAnnotations;
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
            RoleInfo = ObjectMapper.Map<IdentityRoleDto, RoleInfoModel>(
                await _identityRoleAppService.GetAsync(id)
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleUpdateDto>(RoleInfo);
            await _identityRoleAppService.UpdateAsync(RoleInfo.Id, input);

            return NoContent();
        }

        public class RoleInfoModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(IdentityRoleConsts.MaxNameLength)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }
        }
    }
}