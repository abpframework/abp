using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class CreateModalModel : AbpPageModel
    {
        [BindProperty]
        public RoleInfoModel RoleModel { get; set; }

        private readonly IIdentityRoleAppService _identityRoleAppService;

        public CreateModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleCreateDto>(RoleModel);
            await _identityRoleAppService.CreateAsync(input);

            return NoContent();
        }

        public class RoleInfoModel
        {
            [Required]
            [StringLength(IdentityRoleConsts.MaxNameLength)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }
        }
    }
}