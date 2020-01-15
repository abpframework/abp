using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class CreateModalModel : IdentityPageModel
    {
        [BindProperty]
        public RoleInfoModel Role { get; set; }

        private readonly IIdentityRoleAppService _identityRoleAppService;

        public CreateModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleCreateDto>(Role);
            await _identityRoleAppService.CreateAsync(input);

            return NoContent();
        }

        public class RoleInfoModel
        {
            [Required]
            [StringLength(IdentityRoleConsts.MaxNameLength)]
            [Display(Name = "DisplayName:RoleName")]
            public string Name { get; set; }

            [Display(Name = "DisplayName:IsDefault")]
            public bool IsDefault { get; set; }

            [Display(Name = "DisplayName:IsPublic")]
            public bool IsPublic { get; set; }
        }
    }
}