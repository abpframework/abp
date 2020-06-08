using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class CreateModalModel : IdentityPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        protected IIdentityUserAppService IdentityUserAppService { get; }
        protected IIdentityRoleAppService IdentityRoleAppService { get; }

        public CreateModalModel(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
        {
            IdentityUserAppService = identityUserAppService;
            IdentityRoleAppService = identityRoleAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            UserInfo = new UserInfoViewModel();

            var roleDtoList = (await IdentityRoleAppService.GetAllListAsync()).Items;

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(roleDtoList);

            foreach (var role in Roles)
            {
                role.IsAssigned = role.IsDefault;
            }

            return Page();
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserCreateDto>(UserInfo);
            input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();

            await IdentityUserAppService.CreateAsync(input);

            return NoContent();
        }

        public class UserInfoViewModel
        {
            [Required]
            [StringLength(IdentityUserConsts.MaxUserNameLength)]
            public string UserName { get; set; }

            [StringLength(IdentityUserConsts.MaxNameLength)]
            public string Name { get; set; }

            [StringLength(IdentityUserConsts.MaxSurnameLength)]
            public string Surname { get; set; }

            [Required]
            [StringLength(IdentityUserConsts.MaxPasswordLength)]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(IdentityUserConsts.MaxEmailLength)]
            public string Email { get; set; }

            [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
            public string PhoneNumber { get; set; }

            public bool TwoFactorEnabled { get; set; } = true;

            public bool LockoutEnabled { get; set; } = true;
        }

        public class AssignedRoleViewModel
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }

            public bool IsDefault { get; set; }
        }
    }
}
