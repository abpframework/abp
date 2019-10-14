using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class EditModalModel : IdentityPageModel
    {
        [BindProperty]
        public RoleInfoModel Role { get; set; }

        private readonly IIdentityRoleAppService _identityRoleAppService;

        public EditModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Role = ObjectMapper.Map<IdentityRoleDto, RoleInfoModel>(
                await _identityRoleAppService.GetAsync(id)
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleUpdateDto>(Role);
            await _identityRoleAppService.UpdateAsync(Role.Id, input);

            return NoContent();
        }

        public class RoleInfoModel : IHasConcurrencyStamp
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [StringLength(IdentityRoleConsts.MaxNameLength)]
            [Display(Name = "DisplayName:RoleName")]
            public string Name { get; set; }

            [Display(Name = "DisplayName:IsDefault")]
            public bool IsDefault { get; set; }

            public bool IsStatic { get; set; }

            [Display(Name = "DisplayName:IsPublic")]
            public bool IsPublic { get; set; }
        }
    }
}