using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class EditModalModel : AbpPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityRoleAppService _identityRoleAppService;

        public EditModalModel(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
        {
            _identityUserAppService = identityUserAppService;
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(await _identityUserAppService.GetAsync(id));

            Roles = ObjectMapper.Map<List<IdentityRoleDto>, AssignedRoleViewModel[]>(
                await _identityRoleAppService.GetAllListAsync()
            );

            var userRoleNames = (await _identityUserAppService.GetRolesAsync(UserInfo.Id)).Items.Select(r => r.Name).ToList();
            foreach (var role in Roles)
            {
                if (userRoleNames.Contains(role.Name))
                {
                    role.IsAssigned = true;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
            input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
            await _identityUserAppService.UpdateAsync(UserInfo.Id, input);

            return NoContent();
        }

        public class UserInfoViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(IdentityUserConsts.MaxUserNameLength)]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(IdentityUserConsts.MaxEmailLength)]
            [Display(Name = "EmailAddress")]
            public string Email { get; set; }

            [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Display(Name = "TwoFactorVerification")]
            public bool TwoFactorEnabled { get; set; }

            [Display(Name = "AccountLockoutOnFailedLoginAttempts")]
            public bool LockoutEnabled { get; set; }
        }

        public class AssignedRoleViewModel
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }
        }
    }
}