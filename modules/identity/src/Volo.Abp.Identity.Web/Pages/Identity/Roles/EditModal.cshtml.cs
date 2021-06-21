using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class EditModalModel : IdentityPageModel
    {
        [BindProperty]
        public RoleInfoModel Role { get; set; }

        protected IIdentityRoleAppService IdentityRoleAppService { get; }

        public EditModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            IdentityRoleAppService = identityRoleAppService;
        }

        public virtual async Task OnGetAsync(Guid id)
        {
            Role = ObjectMapper.Map<IdentityRoleDto, RoleInfoModel>(
                await IdentityRoleAppService.GetAsync(id)
            );
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleUpdateDto>(Role);
            await IdentityRoleAppService.UpdateAsync(Role.Id, input);

            return NoContent();
        }

        public class RoleInfoModel : ExtensibleObject, IHasConcurrencyStamp
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
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