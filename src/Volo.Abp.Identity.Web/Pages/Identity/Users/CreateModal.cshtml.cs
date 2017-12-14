using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users
{
    public class CreateModalModel : AbpPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityRoleAppService _identityRoleAppService;

        public CreateModalModel(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
        {
            _identityUserAppService = identityUserAppService;
            _identityRoleAppService = identityRoleAppService;
        }

        public async Task OnGetAsync()
        {
            UserInfo = new UserInfoViewModel();

            Roles = ObjectMapper.Map<List<IdentityRoleDto>, AssignedRoleViewModel[]>(
                await _identityRoleAppService.GetAllListAsync()
            );
        }

        public async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserCreateDto>(UserInfo);
            input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();

            await _identityUserAppService.CreateAsync(input);

            return NoContent();
        }

        public class UserInfoViewModel
        {
            [Required]
            [MaxLength(IdentityUserConsts.MaxUserNameLength)]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required]
            [MaxLength(IdentityUserConsts.MaxPasswordLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(IdentityUserConsts.MaxEmailLength)]
            [Display(Name = "EmailAddress")]
            public string Email { get; set; }

            [MaxLength(IdentityUserConsts.MaxPhoneNumberLength)]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }
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
