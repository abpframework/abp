using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class EditModalModel : IdentityPageModel
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

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(
                (await _identityRoleAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items
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

        public class UserInfoViewModel : IHasConcurrencyStamp
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [StringLength(IdentityUserConsts.MaxUserNameLength)]
            public string UserName { get; set; }

            [StringLength(IdentityUserConsts.MaxNameLength)]
            public string Name { get; set; }

            [StringLength(IdentityUserConsts.MaxSurnameLength)]
            public string Surname { get; set; }

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

            public bool TwoFactorEnabled { get; set; }

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